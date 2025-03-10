using BattleGameTester.UI;
using System;
using System.Resources;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


namespace BattleGameTester.Core
{
    [Serializable] public struct Attacks
    {
        public V6 Melee;
        public V6 Range;
        public V6 CC;

        public Attacks (V6 melee, V6 range, V6 cc)
        {
            Melee = melee; Range = range; CC = cc;
        }
        [Serializable] public struct V6
        {
            public ushort Dmg;
            public ushort Def;
            public ushort Coef;
            public ushort AP;
            public ushort Throw;
            public bool Crit;

            public V6(ushort dmg, ushort def, ushort coef, ushort ap, ushort tthrow, bool crit)
            {
                Dmg = dmg; Def = def; Coef = coef; AP = ap; Throw = tthrow; Crit = crit;
            }

        }

    }
    [Serializable] public class Squad : MonoBehaviour, ISquad
    {
        public event Action<string> NameChanged = value => { };
        public event Action<string> AvatarChanged = value => { };
        public event Action<uint> HealthChanged = value => { };
        public event Action<uint> MaxHealthChanged = value => { };
        public event Action<ISquad> Died = value => { };

        public event Action SettingsUpdated;
 

        public Vector3 Position { get => this.transform.position; set { this.transform.position = value; _panelView.SetPosition(value); if (_model == ESquadModels.Image) _imageModel.transform.position = value; } }
        public Quaternion Rotation { get => this.transform.rotation; set => this.transform.rotation = value; }

        #region Parameters
        [SerializeField] private string _squadName;
        public string Name { get => _squadName; set { _squadName = value; NameChanged?.Invoke(value); } }
        [SerializeField] private string _spriteName;
        public string SpriteName { get => _spriteName; 
            set
            {
                _spriteName = value;
                UpdateSpriteTexture();
                AvatarChanged?.Invoke(value);
            } 
        }

        [SerializeField] private uint maxHealth;
        public uint MaxHealth { get => maxHealth; set { maxHealth = value; MaxHealthChanged?.Invoke(maxHealth); } }
        [SerializeField] private uint health;
        public uint Health { get => health; set { health = value; HealthChanged?.Invoke(health);} }
 
        public Attacks Attacks { get; set; }

        public Attacks.V6 GetV6(AttackType type)
        {
            switch (type)
            {
                case AttackType.Melee:
                    return Attacks.Melee;
                case AttackType.Range:
                    return Attacks.Range;
                case AttackType.CC:
                    return Attacks.CC;
                case AttackType.Skip:
                    return new Attacks.V6(0,0,0,0,0,false);
                default:
                    throw new Exception("Unknown attack type GetV6().");
            }
        }
        public ushort GetDmgVal(AttackType type)
        {
            switch (type)
            {
                case AttackType.Melee:
                    return Attacks.Melee.Dmg;
                case AttackType.Range:
                    return Attacks.Range.Dmg;
                case AttackType.CC:
                    return Attacks.CC.Dmg;
                case AttackType.Skip:
                    return 0;
                default:
                    throw new Exception("Unknown attack type GetDmgVal().");
            }
        }
        public ushort GetDefVal(AttackType type)
        {
            switch (type)
            {
                case AttackType.Melee:
                    return Attacks.Melee.Def;
                case AttackType.Range:
                    return Attacks.Range.Def;
                case AttackType.CC:
                    return Attacks.CC.Def;
                case AttackType.Skip:
                    return 0;
                default:
                    throw new Exception("Unknown attack type GetDefVal().");
            }
        }


        private Renderer _renderer;
        private Image _imageModel;
        private ESquadModels _model;
        private ISquadInfoPanelView _panelView;

        #endregion
        private void Awake()
        {
            this.enabled = true;
        }

        private void Start()
        {
            CreatePanelView();
        }
        public void ChangeModelOpacity(int opacity)
        {
            switch (_model)
            {
                case ESquadModels.Image:
                    Debug.Log($"ImageOpacityChanged from {_imageModel.color.a} to {opacity}");
                    var ImageTempColor = _imageModel.color;
                    ImageTempColor.a = opacity / 255f;
                    _imageModel.color = ImageTempColor;
                    break;
                case ESquadModels.Cube:
                    Debug.Log($"Cube Opacity Changed to {opacity}");
                    var CubeTempColor = _renderer.materials[0].color;
                    CubeTempColor.a = opacity / 255f;
                    _renderer.materials[0].SetColor("_BaseColor", CubeTempColor);
                    break;
            }
        }
        public void CreateSquadModel(ESquadModels model)
        {
            if (_imageModel != null) { Destroy(_imageModel.gameObject); _imageModel = null; }
            if (_renderer != null) {Destroy(_renderer.gameObject); _renderer = null; }

            _model = model;
            if (_model == ESquadModels.Cube)
            {
                var cube = CompositionRoot.GetResourceManager().CreatePrefabInstance(_model);
                _renderer = cube.GetComponent<MeshRenderer>();
                cube.transform.position = this.transform.position;
                cube.transform.SetParent(this.transform);
            }
            else if (_model == ESquadModels.Image)
            {
                var image = CompositionRoot.GetResourceManager().CreatePrefabInstance(_model);
                _imageModel = image.GetComponent<Image>();
                image.transform.SetParent(CompositionRoot.GetUIRoot().WorldSpaceCanvas);
                image.transform.position = this.transform.position;
                _imageModel.transform.SetSiblingIndex(1);
            }

            //if (ActivePlayer == EPlayer.P2) model.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); //Make images look left
            //var yOffset = model.GetComponent<MeshFilter>().sharedMesh.bounds.size.y * model.transform.localScale.y / 2;
        }

        public void UpdateSettings(Attacks attacks)
        {
            Attacks = attacks;
            SettingsUpdated?.Invoke();
        }

        public void CopyStats(SquadSave save)
        {
            Name = save.Name;
            Health = save.MaxHealth;
            MaxHealth = save.MaxHealth;
            Attacks = save.Attacks;
            SpriteName = save.SpriteName;
        }


        public void Die()
        {
            CompositionRoot.GetGameHUD().EventLogBox.Log($"Squad \"{_squadName}\" died :(", LogBoxIcons.Death);
            gameObject.SetActive(false);
            _panelView.Hide();
            if (_imageModel != null) { _imageModel.gameObject.SetActive(false); }
        }
        public void Delete()
        {
            CompositionRoot.ShowPopUp($"OK! Squad \"{_squadName}\" removed from battlefield.");
            _panelView.Destroy();
            if (_imageModel != null) { Destroy(_imageModel.gameObject); _imageModel = null; }
            if (_renderer != null) { Destroy(_renderer.gameObject); _renderer = null; }
            Destroy(gameObject);
        }

        #region IHealth
        public void DecreaseHealth(uint value)
        {
            Health -= value;

            if (Health <= 0 || Health > MaxHealth)
            {
                Died(this);
                gameObject.SetActive(false);
            }

        }
        public void IncreaseHealth(uint value)
        {
            Health += value;
        }
        public void RestoreHealth()
        {
            Health = MaxHealth;
        }

        public void SetHealth(uint value)
        {
            Health = value < MaxHealth ? value : MaxHealth;
        }
        #endregion

        private void CreatePanelView()
        {
            var viewFactory = CompositionRoot.GetViewFactory();
            _panelView = viewFactory.CreateSquadInfoPanel();
            _panelView.Init(this);
            AvatarChanged += _panelView.UpdateAvatar;
            NameChanged += _panelView.UpdateName;
            HealthChanged += _panelView.UpdateHP;
            SettingsUpdated += () => _panelView.UpdateInfo(this);

            _panelView.SetParent(CompositionRoot.GetUIRoot().WorldSpaceCanvas);
            _panelView.UpdateInfo(this);
        }
        private void UpdateSpriteTexture()
        {
            if (_model == ESquadModels.Image)
            {
                Debug.Log("NOErr " + _spriteName);
                _imageModel.sprite = CompositionRoot.GetSquadSprite(_spriteName);
            }
            else if (_model == ESquadModels.Cube)
            {
                Debug.Log("Err " + _spriteName);
                var texture = CompositionRoot.GetSquadSprite(_spriteName).texture;
                if (_renderer == null) _renderer = GetComponentInChildren<MeshRenderer>();
                _renderer.materials[0].SetTexture("_BaseMap", texture);
            }
        }


    }
}
