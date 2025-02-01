using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText, targetText, levelText, timeText;
        [SerializeField] private AudioClip timeUpClip, successClip, levelFinish, gameOverClip, mainMenuClip, slideClip;
        [SerializeField] private GameObject mainScene;

        [SerializeField]
        private GameObject mainMenu, targetMenu, mainMenuScene, successMenu, gameOverMenu, shopMenu, statsUI;

        public int levelDuration = 60;
        private int _money;
        public Level Level;
        private float _gameStart, _lastTimeUp;
        private List<Item> _collectedItems = new();
        private static readonly int Set = Animator.StringToHash("set");
        private TextMeshProUGUI _lastEarnText;
        private Animator _lastEarnAnimator;
        private bool _isInLevel;
        private AudioSource _audioSource;

        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                moneyText.text = "$ " + _money.ToString("N0");
            }
        }

        private void Awake()
        {
            _lastEarnText = GameObject.FindWithTag("LastEarn").GetComponent<TextMeshProUGUI>();
            _lastEarnAnimator = GameObject.FindWithTag("LastEarn").GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            foreach (var go in GameObject.FindGameObjectsWithTag("Scene"))
                Destroy(go);
        }

        private void Start()
        {
            _audioSource.clip = mainMenuClip;
            _audioSource.Play();
            mainMenu.SetActive(true);
            foreach (var go in GameObject.FindGameObjectsWithTag("Scene"))
                Destroy(go);
            Instantiate(mainMenuScene);
            statsUI.SetActive(false);
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

                if (leftTime < -0.25)
                {
                    _isInLevel = false;
                    if (Money >= Level.Target)
                    {
                        successMenu.SetActive(true);
                        _audioSource.PlayOneShot(levelFinish);
                        StartCoroutine(ShowShop());

                        IEnumerator ShowShop()
                        {
                            yield return new WaitForSeconds(5);
                            successMenu.SetActive(false);
                            shopMenu.SetActive(true);
                            _audioSource.clip = successClip;
                            _audioSource.Play();
                        }
                    }
                    else
                    {
                        gameOverMenu.SetActive(true);
                        _audioSource.clip = gameOverClip;
                        _audioSource.Play();
                    }

                    Destroy(GameObject.FindWithTag("Scene"));
                    statsUI.SetActive(true);
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
            //TODO account for perks
            if (value > 0)
            {
                Money += value;
                _lastEarnText.text = $"+${value:N0}";
                _lastEarnAnimator.SetTrigger(Set);
            }

            print($"Collected {item.Type}!");
            return value > 0;
        }

        public void LoadLevel(Level level)
        {
            Level = level;
            _audioSource.Stop();
            _audioSource.PlayOneShot(slideClip);
            mainMenu.SetActive(false);
            targetMenu.SetActive(true);
            foreach (var go in GameObject.FindGameObjectsWithTag("Scene"))
                Destroy(go);
            StartCoroutine(StartLevel());
            return;

            IEnumerator StartLevel()
            {
                yield return new WaitForSeconds(5);
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
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}