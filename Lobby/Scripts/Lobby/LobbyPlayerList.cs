using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
    public class LobbyPlayerList : NetworkBehaviour
    {
        public static LobbyPlayerList _instance = null;

        public RectTransform playerListContentTransform;
        public GameObject warningDirectPlayServer;
        public Transform addButtonRow;
        public GameObject[] Character;
        public GameObject[] CharacterHasBeenChoice;
        public GameObject _PreviousChoose;
        public GameObject _PreviousHasBeenChoose;
        public GameObject[] Idle;
        public GameObject[] IntroText;
        public GameObject LastIntro;
        public int CharacterID = -1;
        public int PreviousID;
        public bool _lock;
        public bool CanChoose;
        public GameObject NowChoose;
        public GameObject _lockButton;
        LobbyPlayer _player;
        public GameObject[] ModeIntro;
        public Dropdown _drop;
        public int modeNum;


        protected VerticalLayoutGroup _layout;
        protected List<LobbyPlayer> _players = new List<LobbyPlayer>();
        [SyncVar]
        public int lockCount = 0;

        void Start()
        {
            CanChoose = true;
            _lockButton.SetActive(false);
        }

        public void OnEnable()
        {
            _instance = this;
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }


        void Update()
        {
            //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
            //sometime to child being assigned before layout was enabled/init, leading to broken layouting)
            
            if(_layout)
                _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
            if (modeNum != 0 && _lock == true)
            {
                _player.readyButton.gameObject.SetActive(true);
            }
            else
                return;
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListContentTransform, false);
            addButtonRow.transform.SetAsLastSibling();

            PlayerListModified();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            _players.Remove(player);
            PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach (LobbyPlayer p in _players)
            {
                p.OnPlayerListChanged(i);
                ++i;
                
            }
        }

        public void ChooseCharacter(int ChooseID)
        {
            if (CanChoose == true)
            {
                _lockButton.SetActive(true);
                if (_player == null)
                {
                    _player = GameObject.Find("ME").GetComponent<LobbyPlayer>();
                }
                CharacterID = ChooseID;
                if (_PreviousChoose == null)
                {
                    CharacterHasBeenChoice[CharacterID].SetActive(true);
                    Character[CharacterID].SetActive(false);
                    _PreviousChoose = Character[CharacterID];
                    _PreviousHasBeenChoose = CharacterHasBeenChoice[CharacterID];
                    NowChoose = CharacterHasBeenChoice[CharacterID];
                    IntroText[CharacterID].SetActive(true);
                    LastIntro = IntroText[CharacterID];
                    PreviousID = -1;
                    
                }
                else if (_PreviousChoose != null)
                {
                    CharacterHasBeenChoice[CharacterID].SetActive(true);
                    Character[CharacterID].SetActive(false);
                    _PreviousChoose.SetActive(true);
                    for (int i = 0; i < Character.Length; i++)
                    {
                        if (Character[i] == _PreviousChoose)
                        {
                            PreviousID = i;
                        }
                    }
                    _PreviousHasBeenChoose.SetActive(false);
                    _PreviousChoose = Character[CharacterID];
                    LastIntro.SetActive(false);
                    IntroText[CharacterID].SetActive(true);
                    LastIntro = IntroText[CharacterID];
                }
                _player.CmdCharacterChoose(CharacterID, PreviousID);
            }

        }

        public void Lock()
        {
            if (NowChoose != null)
            {
                _lockButton.SetActive(false);
                if (_player == null)
                {
                    _player = GameObject.Find("ME").GetComponent<LobbyPlayer>();
                }
                CanChoose = false;
                _lock = true;
                _player.CmdLockplayer(CharacterID);
            }
        }
        
        public void ChangeValue()
        {
            if (_player == null)
            {
                _player = GameObject.Find("ME").GetComponent<LobbyPlayer>();
            }
            if (_drop.value == 0)
            {
                ModeIntro[0].SetActive(false);
                ModeIntro[1].SetActive(false);
                ModeIntro[2].SetActive(false);
                modeNum = 0;
            }
            else if (_drop.value == 1)
            {
                ModeIntro[0].SetActive(true);
                ModeIntro[1].SetActive(false);
                ModeIntro[2].SetActive(false);
                modeNum = 1;
                GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene = "MAP1";
            }
            else if (_drop.value == 2)
            {
                ModeIntro[0].SetActive(false);
                ModeIntro[1].SetActive(true);
                ModeIntro[2].SetActive(false);
                modeNum = 2;
                GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene = "MAP2";
            }
            else if (_drop.value == 3)
            {
                ModeIntro[0].SetActive(false);
                ModeIntro[1].SetActive(false);
                ModeIntro[2].SetActive(true);
                modeNum = 3;
                GameObject.Find("LobbyManager").GetComponent<NetworkLobbyManager>().playScene = "MAP3";
            }
            _player.CmdChooseMode(modeNum);
           
        }
    }
}
