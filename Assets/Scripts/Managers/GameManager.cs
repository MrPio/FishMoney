using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Prefabs;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Item = Model.Item;
using Level = Model.Level;

namespace Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText, targetText, levelText, timeText;

        [SerializeField]
        private AudioClip timeUpClip, successClip, levelFinish, gameOverClip, mainMenuClip, slideClip, winClip;

        [SerializeField] private GameObject mainScene;
        [SerializeField] private bool debugMode;

        [SerializeField] private GameObject mainMenu,
            targetMenu,
            mainMenuScene,
            successMenu,
            gameOverMenu,
            winMenu,
            shopMenu,
            statsUI,
            inputHUD;

        public int levelDuration = 60;
        private int _bombs = 0, _magnifier = 0;
        public bool HasWater, HasRotation, HasClover, HasCreditCard;
        private int _money;
        public Level Level;
        private float _gameStart, _lastTimeUp;
        private List<Item> _collectedItems = new();
        private static readonly int Set = Animator.StringToHash("set");
        private TextMeshProUGUI _lastEarnText;
        private Animator _lastEarnAnimator;
        private bool _isInLevel;
        private AudioSource _audioSource;

        private BombsContainer BombsContainer =>
            GameObject.FindWithTag("BombsContainer").GetComponent<BombsContainer>();

        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                moneyText.text = "$ " + _money.ToString("N0");
            }
        }

        public int Bombs
        {
            get => _bombs;
            set
            {
                _bombs = value;
                if (_isInLevel)
                    BombsContainer.SetBombs(_bombs);
            }
        }

        public int Magnifier
        {
            get => _magnifier;
            set
            {
                _magnifier = value;
                if (_isInLevel)
                    SetPerks();
            }
        }

        private void Awake()
        {
            _lastEarnText = GameObject.FindWithTag("LastEarn").GetComponent<TextMeshProUGUI>();
            _lastEarnAnimator = GameObject.FindWithTag("LastEarn").GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            if (!debugMode)
            {
                foreach (var go in GameObject.FindGameObjectsWithTag("Scene"))
                    Destroy(go);
            }
        }

        private void Start()
        {
            if (debugMode) return;
            _audioSource.clip = mainMenuClip;
            _audioSource.Play();
            mainMenu.SetActive(true);
            foreach (var go in GameObject.FindGameObjectsWithTag("Scene"))
                Destroy(go);
            Instantiate(mainMenuScene);
            statsUI.SetActive(false);
            inputHUD.SetActive(false);
            if (InputManager.IsMobile)
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
            }
        }

        private void FixedUpdate()
        {
            if (_isInLevel)
            {
                var leftTime = levelDuration - (Time.time - _gameStart);
                timeText.text = math.clamp(leftTime, 0, levelDuration).ToString("N0");
                if (leftTime <= 10 && Time.time - _lastTimeUp >= 1)
                {
                    timeText.color = Color.Lerp(Color.white, Color.red, 0.75f);
                    _audioSource.PlayOneShot(timeUpClip);
                    _lastTimeUp = Time.time;
                }

                // End of level
                if (leftTime < -0.25)
                {
                    _isInLevel = false;
                    Magnifier = 0;
                    HasWater = false;
                    HasRotation = false;
                    HasClover = false;
                    HasCreditCard = false;

                    // Success
                    if (Money >= Level.Target)
                    {
                        if (Level.Id >= Level.Levels.Length)
                        {
                            winMenu.SetActive(true);
                            _audioSource.clip = winClip;
                            _audioSource.Play();
                        }
                        else
                        {
                            successMenu.SetActive(true);
                            _audioSource.PlayOneShot(levelFinish);
                            StartCoroutine(ShowShop());

                            IEnumerator ShowShop()
                            {
                                yield return new WaitForSeconds(4);
                                successMenu.SetActive(false);
                                shopMenu.SetActive(true);
                                _audioSource.clip = successClip;
                                _audioSource.Play();
                            }
                        }
                    }
                    // Game Over
                    else
                    {
                        gameOverMenu.SetActive(true);
                        _audioSource.clip = gameOverClip;
                        _audioSource.Play();
                    }

                    Destroy(GameObject.FindWithTag("Scene"));
                    statsUI.SetActive(false);
                    inputHUD.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        public bool CollectItem(Item item)
        {
            if (item is null) return false;
            _collectedItems.Add(item);
            var value = item.Value;

            if (value > 0)
            {
                // Magnifier
                if (Magnifier > 0 && item.Type is ItemType.RockS or ItemType.RockL)
                {
                    Magnifier -= 1;
                    if (item.Type is ItemType.RockS) value = 300;
                    else if (item.Type is ItemType.RockL) value = 500;
                }

                // Credit Card
                if (HasCreditCard && item.Type is ItemType.Diamond or ItemType.BirdDiamond)
                    value += (int)(value * 0.5);

                // Clover
                if (HasClover && item.Type is ItemType.Sack or ItemType.Treasure)
                    value += (int)(value * 0.35);

                Money += value;
                _lastEarnText.text = $"+${value:N0}";
                _lastEarnAnimator.SetTrigger(Set);
            }
            else if (item.Type is ItemType.Bomb)
                Bombs++;

            print($"Collected {item.Type}!");
            return value > 0 && item.Type is not ItemType.Bomb;
        }

        public void LoadLevel(Level level)
        {
            Level = level;
            _audioSource.Stop();
            _audioSource.PlayOneShot(slideClip);
            mainMenu.SetActive(false);
            shopMenu.SetActive(false);
            targetMenu.SetActive(true);
            foreach (var go in GameObject.FindGameObjectsWithTag("Scene"))
                Destroy(go);
            StartCoroutine(StartLevel());
            return;

            IEnumerator StartLevel()
            {
                yield return new WaitForSeconds(4);
                targetMenu.SetActive(false);
                levelText.text = Level.Id.ToString();
                targetText.text = "$ " + Level.Target.ToString("N0");
                moneyText.text = "$ " + _money.ToString("N0");
                _gameStart = Time.time;
                _isInLevel = true;
                var sceneGo = Instantiate(mainScene);
                var levelGo = Resources.Load<GameObject>("Prefabs/Levels/" + Level.Id.ToString("D3"));
                Instantiate(levelGo, sceneGo.transform);
                statsUI.SetActive(true);
                inputHUD.SetActive(InputManager.IsMobile);
                BombsContainer.SetBombs(_bombs);
                SetPerks();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void SetPerks()
        {
            var perksContainer = GameObject.FindWithTag("PerksContainer").transform;
            perksContainer.Find("water_container").gameObject.SetActive(HasWater);
            perksContainer.Find("rotation_container").gameObject.SetActive(HasRotation);
            perksContainer.Find("clover_container").gameObject.SetActive(HasClover);
            perksContainer.Find("credit_card_container").gameObject.SetActive(HasCreditCard);
            perksContainer.Find("magnifier_container").gameObject.SetActive(Magnifier > 0);
        }
    }
}