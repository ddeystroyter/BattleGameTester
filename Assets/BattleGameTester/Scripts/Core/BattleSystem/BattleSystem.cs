using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleGameTester.UI;
using System.Linq;

namespace BattleGameTester.Core 
{
    public class BattleSystem : MonoBehaviour, IBattleSystem
    {
        public Dictionary<Vector2Int, ISquad> SquadCells;

        private IGameHUD gameHUD;
        private IGridSystem gridSystem;
        private IViewFactory viewFactory;
        private IUserInput UserInput;
        private ISquad attackingSquad;
        private ISquad defendingSquad;
        private IGameSettings gameSettings;
        private IAudioManager audioManager;

        private Dictionary<Tuple<EPlayer, AttackType>, List<CombatMove>> Combat = new Dictionary<Tuple<EPlayer, AttackType>, List<CombatMove>>();

        private List<ISquad> SquadsToDie = new List<ISquad>();
        private Dictionary<EPlayer, AttackType> ActiveTurnType = new Dictionary<EPlayer, AttackType>();

        private IAttackTypeChoiceView attackTypeChoiceView;

        private Transform squadParent;
        public EPlayer ActivePlayer
        {
            get => _activePlayer;
            set { _activePlayer = value; gridSystem.ActivePlayer = value; }
        }
        private EPlayer _activePlayer;
        private ESquadModels _modelsMode;
        private int _modelsOpacity;

        private void Start()
        {
            SquadCells = new Dictionary<Vector2Int, ISquad>();

            gameHUD = CompositionRoot.GetGameHUD();
            gridSystem = CompositionRoot.GetGridSystem();
            viewFactory = CompositionRoot.GetViewFactory();
            UserInput = CompositionRoot.GetUserInput();
            gameSettings = CompositionRoot.GetGameSettings();
            audioManager = CompositionRoot.GetAudioManager();

            squadParent = new GameObject("SquadParent").transform;

            Enum.TryParse(gameSettings.ModelsMode, out _modelsMode);
            _modelsOpacity = gameSettings.ModelsOpacity;
            gameSettings.ModelsModeChanged += OnModelsModeChanged;
            gameSettings.ModelsOpacityChanged += OnModelsOpacityChanged;

            attackTypeChoiceView = viewFactory.CreateAttackTypeChoice();
            attackTypeChoiceView.Hide();
            attackTypeChoiceView.CloseBtn_Clicked += OnAttackTypeChoiceView_Closed;
            attackTypeChoiceView.AttackType_Confirmed += OnAttackType_Clicked;

            foreach (var player in (EPlayer[])Enum.GetValues(typeof(EPlayer)))
            {
                ActiveTurnType.Add(player, AttackType.Range);
                foreach (var roundType in new AttackType[] { AttackType.Melee, AttackType.Range, AttackType.CC })
                {
                    Combat.Add(new Tuple<EPlayer, AttackType>(player, roundType), new List<CombatMove>());
                }
            }
            ActivePlayer = EPlayer.P1;

            gameHUD.CombatQueue.Show(ActivePlayer, ActiveTurnType[ActivePlayer], Combat[new Tuple<EPlayer, AttackType>(ActivePlayer, AttackType.Melee)]);
            gameHUD.RoundIndicator.UpdateRound(EPlayer.P1, ActiveTurnType[EPlayer.P1]);
            gameHUD.RoundIndicator.UpdateRound(EPlayer.P2, ActiveTurnType[EPlayer.P2]);
            gameHUD.Buttons.PlayerChange_Clicked += ChangeActivePlayer;
            gameHUD.Buttons.MakeTurn_Clicked += MakeTurn;
            gameHUD.Buttons.SkipTurn_Clicked += SkipTurn;
            gameHUD.Buttons.DiceRoll_Clicked += () => MakeDiceRoll(1);
            UserInput.Z_Pressed += ChangeActivePlayer;
            UserInput.Q_Pressed += MakeTurn;
            UserInput.W_Pressed += SkipTurn;
            UserInput.E_Pressed += () => MakeDiceRoll(1);
            gridSystem.CursorEnterNotEmptyCell += (cell) => gameHUD.SquadOverviewPanel.Show(SquadCells[cell]);
            gridSystem.CursorEnterEmptyCell += gameHUD.SquadOverviewPanel.Hide;
        }

        public ISquad GetActiveSquad()
        {
            return SquadCells[gridSystem.activeCell];
        }
        public void AddDefaultSquad()
        {
            var name = "Default";
            uint maxHealth = 1000;
            var attacks = new Attacks(new Attacks.V6(1, 1, 10, 1, 3, true), new Attacks.V6(1, 1, 10, 1, 3, true), new Attacks.V6(1, 1, 10, 1, 3, true));
            var spriteName = "default.jpg";
            AddSquad(new SquadSave(name, spriteName, maxHealth, attacks));
        }
        public void AddSquad(SquadSave ss)
        {
            var squad = MonoExtensions.CreateComponent<Squad>();
            squad.transform.SetParent(squadParent);
            squad.Died += OnSquadDied;

            var yOffset = 0.15f;
            squad.transform.position = gridSystem.GetActiveCellWorldPosition() + new Vector3(0f, yOffset, 0f);
            squad.CreateSquadModel(_modelsMode);
            squad.ChangeModelOpacity(_modelsOpacity);
            SquadCells.Add(gridSystem.GetActiveCell(), squad);
            gridSystem.MakeActiveCellNotEmpty();
            squad.CopyStats(ss);
            audioManager.PlayEffect(EAudio.Squad_Added);
            gridSystem.CloseCell();
        }
        public void DeleteActiveSquad()
        {
            var activeSquad = GetActiveSquad();
            DeleteSquad(activeSquad);
        }
        public void MoveSquad()
        {
            gridSystem.State = GridState.MovePick;
            gridSystem.CellPicked += (cell) => MoveSquad(gridSystem.GetActiveCell(), cell);
        }
        public void ChooseTargetSquad()
        {
            var inputCell = gridSystem.GetActiveCell();
            gridSystem.State = GridState.TargetPick;
            gridSystem.CellPicked += (cell) => ChooseTargetSquad(inputCell, cell);
        }
        private void OnModelsModeChanged(string mode)
        {
            Enum.TryParse(mode, out _modelsMode);
            foreach (ISquad squad in SquadCells.Values)
            {
                squad.CreateSquadModel(_modelsMode);
                squad.SpriteName = squad.SpriteName;
            }
            OnModelsOpacityChanged(_modelsOpacity);
        }
        private void OnModelsOpacityChanged(int opacity)
        {
            _modelsOpacity = opacity;
            foreach (ISquad squad in SquadCells.Values)
            {
                squad.ChangeModelOpacity(opacity);
            }
        }
        private void ChangeActivePlayer()
        {
            ActivePlayer = ActivePlayer == EPlayer.P1 ? EPlayer.P2 : EPlayer.P1;
            gameHUD.CombatQueue.Show(ActivePlayer, ActiveTurnType[ActivePlayer], Combat[new Tuple<EPlayer, AttackType>(ActivePlayer, ActiveTurnType[ActivePlayer])]);
            gridSystem.State = GridState.CellPick;
            Debug.Log("Player_changed: " + ActivePlayer.ToString());
        }
        private void OnAttackTypeChoiceView_Closed()
        {
            UserInput.Escaped -= OnAttackTypeChoiceView_Closed;
            attackTypeChoiceView.Hide();
        }
        private void OnAttackType_Clicked(AttackType attackType)
        {
            OnAttackTypeChoiceView_Closed();
            if (ActiveTurnType[ActivePlayer] != attackType && attackType != AttackType.Skip)
            {
                CompositionRoot.ShowPopUp($"NOT OK! Not {ActiveTurnType[ActivePlayer]} attacks are not allowed in {ActiveTurnType[ActivePlayer]} Round!");
                return;
            }
            audioManager.PlayEffect(EAudio.Squad_TargetSelected);


            AddCombatMove(ActivePlayer, new CombatMove(attackingSquad, defendingSquad, attackType));
            gridSystem.State = GridState.CellPick;
        }
        private void AddCombatMove(EPlayer player, CombatMove newCM)
        {
            var player_round = new Tuple<EPlayer, AttackType>(player, ActiveTurnType[player]); 
            if (Combat[player_round].Exists(cm => cm.AttackingSquad == newCM.AttackingSquad))
            {
                var oldPriority = Combat[player_round].Find(cm => cm.AttackingSquad == newCM.AttackingSquad).Priority;
                Combat[player_round].RemoveAll(cm => cm.AttackingSquad == newCM.AttackingSquad);
                foreach (var cm in Combat[player_round].FindAll(cm => cm.Priority > oldPriority))
                {
                    cm.Priority -= 1;
                }
                gameHUD.CombatQueue.Show(player, ActiveTurnType[player], Combat[player_round]);
            }

            if (Combat[player_round].Count > 0) newCM.Priority = (byte)(Combat[player_round].Max(cm => cm.Priority) + 1);
            else newCM.Priority = 0;

            Combat[player_round].Add(newCM);

            gameHUD.CombatQueue.AddCombatMove(newCM, true);

        }

        #region SquadOperations
        private void OnSquadDied(ISquad squad)
        {
            SquadsToDie.Add(squad);
            squad.Die();
        }
        private void DeleteSquad(ISquad squad)
        {
            foreach (var cmList in Combat.Values)
            {
                cmList.RemoveAll(x => x.DefendingSquad == squad || x.AttackingSquad == squad);
            }
 
            squad.Delete();
            Vector2Int squadCell = new Vector2Int(100, 100);
            foreach (var kvp in SquadCells)
            {
                if (kvp.Value == squad)
                {
                    squadCell = kvp.Key;
                    gridSystem.SetCellEmptyState(squadCell, true);
                    SquadCells.Remove(kvp.Key);
                    return;
                }
            };
        }
        private void MoveSquad(Vector2Int from, Vector2Int to)
        {
            Debug.Log($"Moved From {from}, to {to}");
            SquadCells.Add(to, SquadCells[from]);
            SquadCells.Remove(from);
            var squad = SquadCells[to];

            //var model = squad.GetClass().GetComponentInChildren<MeshFilter>();
            //var yOffset = model.sharedMesh.bounds.size.y * model.transform.localScale.y / 2;

            squad.Position = gridSystem.GetCellWorldPosition(to) + new Vector3(0f, 0.15f, 0f);
            gridSystem.State = GridState.CellPick;
        }
        private void ChooseTargetSquad(Vector2Int attacking, Vector2Int attacked)
        {
            gridSystem.State = GridState.Locked;
            attackingSquad = SquadCells[attacking];
            defendingSquad = SquadCells[attacked];
            gridSystem.CellPicked -= (cell) => ChooseTargetSquad(attacking, attacked);
            attackTypeChoiceView.Show(attackingSquad, defendingSquad);
            UserInput.Escaped += OnAttackTypeChoiceView_Closed;
        }
        #endregion

        #region Combat
        private void MakeTurn()
        {
            var player_round = new Tuple<EPlayer, AttackType>(ActivePlayer, ActiveTurnType[ActivePlayer]);
            if (!Combat.ContainsKey(new Tuple<EPlayer, AttackType> (ActivePlayer, ActiveTurnType[ActivePlayer]))) return;
            gameHUD.CombatQueue.Hide();
            gameHUD.Buttons.Lock();
            UserInput.Lock();
            gridSystem.State = GridState.Locked;

            var cms = Combat[player_round];
            cms.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            StartCoroutine(Attacking(cms));
        }
        private void SkipTurn()
        {
            gameHUD.EventLogBox.Log($"\"{ActivePlayer}\" пропускает ход {ActiveTurnType[ActivePlayer]}.", LogBoxIcons.Skip);
            NextTurn();
        }
        private void NextTurn()
        {
            switch (ActiveTurnType[ActivePlayer])
            {
                case AttackType.Range:
                    ActiveTurnType[ActivePlayer] = AttackType.Melee;
                    break;
                case AttackType.Melee:
                    ActiveTurnType[ActivePlayer] = AttackType.CC;
                    break;
                case AttackType.CC:
                    ActiveTurnType[ActivePlayer] = AttackType.Range;
                    break;
                default: break;
            }
            gameHUD.RoundIndicator.UpdateRound(ActivePlayer, ActiveTurnType[ActivePlayer]);
            ChangeActivePlayer();
        }
        private IEnumerator Attacking(List<CombatMove> cms)
        {
            foreach (var cm in cms)
            {

                if (SquadsToDie.Any(x => x == cm.AttackingSquad)) 
                {
                    gameHUD.EventLogBox.Log($"Отряд \"{cm.AttackingSquad.Name}\" не в состоянии атаковать.");
                    continue; 
                }
                else if (SquadsToDie.Any(x => x == cm.DefendingSquad))
                {
                    gameHUD.EventLogBox.Log($"Отряд \"{cm.DefendingSquad.Name}\" не в состоянии защищаться.");
                    continue;
                }

                if (cm.AttackType == AttackType.Skip)
                {
                    gameHUD.EventLogBox.Log($"Отряд \"{cm.AttackingSquad.Name}\" делает пропуск хода.");
                    audioManager.PlayEffect(EAudio.Combat_Skip);
                    continue;
                }

                var attackV6 = cm.AttackingSquad.GetV6(cm.AttackType);
                var defenseV6 = cm.DefendingSquad.GetV6(cm.AttackType);


                //Прямая атака
                gameHUD.EventLogBox.Log($"Прямая атака ({cm.AttackType}) -> Отряд \"{cm.AttackingSquad.Name}\" атакует  \"{cm.DefendingSquad.Name}\"");
                var dmg = CalculateDmg(attackV6.Throw, attackV6.Dmg, defenseV6.Def, attackV6.AP, attackV6.Coef, attackV6.Crit);
                if (dmg > 0) audioManager.PlayEffect(cm.AttackType);
                cm.DefendingSquad.DecreaseHealth(dmg);
                yield return new WaitForSeconds(2f);

                if (ActiveTurnType[ActivePlayer] == AttackType.Range) continue;
                if (SquadsToDie.Any(x => x == cm.DefendingSquad)) { continue; }

                //Обратная атака
                gameHUD.EventLogBox.Log($"Обратная атака ({cm.AttackType}) -> Отряд \"{cm.DefendingSquad.Name}\" атакует в ответ \"{cm.AttackingSquad.Name}\"");
                dmg = CalculateDmg(defenseV6.Throw, defenseV6.Dmg, attackV6.Def, defenseV6.AP, defenseV6.Coef, defenseV6.Crit);
                if (dmg > 0) audioManager.PlayEffect(cm.AttackType);
                cm.AttackingSquad.DecreaseHealth(dmg);

                yield return new WaitForSeconds(3f);

            }

            foreach (var squad in SquadsToDie)
            {
                DeleteSquad(squad);
            }
            SquadsToDie.Clear();

            NextTurn();
        }
        private uint CalculateDmg(ushort diceSide, ushort diceAmount, ushort def, ushort ap, ushort coef, bool critOn)
        {
            Debug.Log($"Dice:{diceSide}; Def:{def};AP{ap};Coef:{coef}");
            var attackRollDict = MakeDiceRoll(diceAmount);
            var attackSum = attackRollDict.Sum(side => side.Key >= diceSide && side.Key < 6 ? side.Value : 0);
            var critSum = attackRollDict.ContainsKey(6) ? attackRollDict[6] : 0;
            if (!critOn) { attackSum += critSum; critSum = 0; }

            if (ap > def) { def = 0; ap = 0; }
            uint total = (uint)(Mathf.Max(attackSum - (def - ap), 0) + critSum) * coef;
            if (total == 0) { audioManager.PlayEffect(EAudio.Combat_ArmorSaved_1); }
            var critText = critOn ? $"(крит. {critSum * coef})" : "(крит. выкл.)";
            gameHUD.EventLogBox.Log($"Нанесено урона - {total} {critText}.", LogBoxIcons.Damage);
            return total;
        }
        private Dictionary<byte, ushort> MakeDiceRoll(ushort amount)
        {
            var rollDict = new Dictionary<byte, ushort>();
            for (int i = 0; i < amount; i++) 
            {
                var side = (byte)UnityEngine.Random.Range(1, 7);
                if (rollDict.ContainsKey(side)) rollDict[side] += 1;
                else rollDict.Add(side, 1);
            }

            //Debug
            var dbgStr = $"DiceRoll ({amount}) >>> ";
            foreach (var kvp in rollDict.OrderBy(x => x.Key))
            {
                dbgStr += $"Side_{kvp.Key}: {kvp.Value} || ";
            }
            gameHUD.EventLogBox.Log(dbgStr, LogBoxIcons.DiceRoll);

            return rollDict;
        }
        #endregion
    }
}

