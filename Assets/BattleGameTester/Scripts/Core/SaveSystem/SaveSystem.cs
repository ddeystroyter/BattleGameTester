using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BattleGameTester.UI;
using System;
using System.Linq;

namespace BattleGameTester.Core
{
    [Serializable] public struct SquadSave
    {
        public string Name;
        public string SpriteName;
        public uint MaxHealth;
        public Attacks Attacks;
        public SquadSave(string name, string spriteName, uint maxHealth, Attacks attacks)
        {
            Name = name;
            SpriteName = spriteName;
            MaxHealth = maxHealth;
            Attacks = attacks;
        }
    }
    public class SaveSystem : MonoBehaviour, ISaveSystem
    {
        public event Action<SquadSave> Squad_Added;
        public event Action SavedSquadsView_Closed;

        private static ISavedSquadsView _savedSquadsView;
        private IResourceManager ResourceManager;
        private ISquad _squad;
        private IUserInput _userInput;
        private static List<SquadSave> squads;

        private static readonly string _saveDirectory = Application.dataPath + "/Saves/";
        private static string _squadsSaveFile;
        private static string _separator = "\n";

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();
            _userInput = CompositionRoot.GetUserInput();

            var viewFactory = CompositionRoot.GetViewFactory();
            _savedSquadsView = viewFactory.CreateSavedSquads();
            _savedSquadsView.SetParent(CompositionRoot.GetUIRoot().OverlayCanvas);
            _savedSquadsView.RefreshBtn_Clicked += RefreshSavedSquads;
            _savedSquadsView.CloseBtn_Clicked += SavedSquadsHide;

            _squadsSaveFile = _saveDirectory + "squadsSave.data";
            if (!Directory.Exists(_saveDirectory)) Directory.CreateDirectory(_saveDirectory);
            if (!File.Exists(_squadsSaveFile))
            {
                File.Create(_squadsSaveFile);
                CompositionRoot.ShowPopUp("WARNING! New SaveFile Created!");
            }

            LoadSquads();
        }
        public void ShowSavedSquads()
        {
            LoadSquads();
            _savedSquadsView.ClearContent();
            _userInput.Escaped += SavedSquadsHide;
            _savedSquadsView.Show();

            for (int i = 0; i < squads.Count; i++)
            {
                var item = ResourceManager.CreatePrefabInstance<ISavedSquadsView_Item, EUI_Items>(EUI_Items.SavedSquadsView_Item);
                item.SetParent(_savedSquadsView.Content);
                var spriteName = squads[i].SpriteName;
                var name = squads[i].Name;
                var hp = new Vector2Int((int)squads[i].MaxHealth, (int)squads[i].MaxHealth);
                var attacks = squads[i].Attacks;

                item.Create(i, spriteName, name, hp, attacks);
                if (_squad != null) { item.AddBtn_Clicked += (id) => _squad.CopyStats(squads[id]); }
                else { item.AddBtn_Clicked += (id) => { Squad_Added?.Invoke(squads[id]); SavedSquadsHide(); }; }
                item.DeleteBtn_Clicked += (id) => DeleteSquad(id);

            }
        }
        public void SaveSquad(ISquad squad)
        {
            var save = new SquadSave();
            save.Name = squad.Name;
            save.MaxHealth = squad.MaxHealth;
            save.Attacks = squad.Attacks;
            save.SpriteName = squad.SpriteName;


            //Check if squad in SaveFile already 
            var lines = File.ReadAllText(_squadsSaveFile).Split(_separator);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "") continue;
                try
                {
                    var s = JsonUtility.FromJson<SquadSave>(lines[i]);
                    if (s.Name == squad.Name)
                    {
                        lines[i] = JsonUtility.ToJson(save);
                        File.WriteAllText(_squadsSaveFile, string.Join(_separator, lines));
                        CompositionRoot.ShowPopUp("OK! Save overwritten: " + s.Name);
                        //Debug.Log("OverWrite Save: " + s.Name);
                        return;
                    }
                }
                catch
                {
                    CompositionRoot.ShowPopUp($"ERROR! Overwrite SAVE {squad.Name}");
                    lines[i] = null;
                    continue;
                }

            }
            CheckEndLine();
            File.AppendAllText(_squadsSaveFile, JsonUtility.ToJson(save) + _separator);
            CompositionRoot.ShowPopUp("OK! New Save: " + squad.Name);
            //Debug.Log("✔ New Save: " + squad.Name);
            LoadSquads();
        }


        private void RefreshSavedSquads()
        {
            LoadSquads();
            _savedSquadsView.ClearContent();
            ShowSavedSquads();
        }
        private void LoadSquads()
        {
            squads = new List<SquadSave>();
            CheckEndLine();
            var lines = File.ReadAllText(_squadsSaveFile).Split(_separator);
            for (int i = 0; i < lines.Length; i++)
            {
                    try
                    {
                        if (lines[i].Length > 0) squads.Add(JsonUtility.FromJson<SquadSave>(lines[i]));
                    }
                    catch
                    {
                        lines[i] = null;
                        File.WriteAllText(_squadsSaveFile, string.Join(_separator, lines.Where(s => !string.IsNullOrEmpty(s))));
                        CompositionRoot.ShowPopUp("ERROR! SAVE FILE. Problem lines was deleted!");
                        LoadSquads();
                        return;
                    }
            }
            squads = squads.OrderBy(squad => squad.Name).ToList();
            Debug.Log("Squads Loaded. Amount =" + squads.Count);
        }
        private void CheckEndLine() 
        {
            var lines = File.ReadAllText(_squadsSaveFile).Split(_separator);
            if (string.IsNullOrEmpty(lines[lines.Length - 1])) return;
            else File.AppendAllText(_squadsSaveFile, _separator);
        }
        private void DeleteSquad(int id)
        {
            //Check if squad in SaveFile already 
            var lines = File.ReadAllText(_squadsSaveFile).Split(_separator);
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    var s = JsonUtility.FromJson<SquadSave>(lines[i]);
                    if (s.Name == squads[id].Name)
                    {
                        lines[i] = null;
                        CompositionRoot.ShowPopUp("OK! Save Deleted: " + s.Name);
                        //Debug.Log("✔ Deleted Save: " + s.Name);
                        break;
                    }
                }
                catch
                {
                    lines[i] = null;
                    CompositionRoot.ShowPopUp("ERROR! Damaged Save DELETED!");
                }

            }
            File.WriteAllText(_squadsSaveFile, string.Join(_separator, lines.Where(s => !string.IsNullOrEmpty(s))));
            //items[index].Destroy();
            //items.RemoveAt(index);
            squads.RemoveAt(id);
            RefreshSavedSquads();
        }
        private void SavedSquadsHide()
        {
            _userInput.Escaped -= SavedSquadsHide;
            _savedSquadsView.Hide();
            SavedSquadsView_Closed?.Invoke();
        }
    }
}

