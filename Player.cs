using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float KeepRecVal;
    public int SJ_Count;
    public float[] PlayaPos;
    public bool KeepISPlayaInNormalMode;
    public float[] RedSealPos;
    public float[] BlueSealPos;
    public static string SaveString;
    GameObject RedSealObj, BlueSealObj;
    public float[] ThelosNorModeVel;
    Rigidbody TP_RB;
    public string HoldStateName;
    public float HoldStateProgress;
    public float[][][] SenRotations;
    public static bool ALoadHasOccured;
    // Start is called before the first frame update
    private void Awake()
    {
        ALoadHasOccured = false;
        SenRotations = new float[11][][];
        HoldStateName = "Idle";
        HoldStateProgress = 0;
        SaveString = "";
        PlayaPos = new float[3];
        RedSealPos = new float[3];
        BlueSealPos = new float[3];
        ThelosNorModeVel = new float[3];
        SJ_Count = 0;
        if (PlayerPrefs.HasKey("ReLoadSlot1FromMain"))
        {
            if (PlayerPrefs.GetInt("ReLoadSlot1FromMain") == 1)
            {
                StartCoroutine(ReloadSlot1());
            }
        }
        if (PlayerPrefs.HasKey("ReLoadSlot2FromMain"))
        {
            if (PlayerPrefs.GetInt("ReLoadSlot2FromMain") == 1)
            {
                StartCoroutine(ReloadSlot2());
            }
        }
    }
    private void FixedUpdate()
    {
        ALoadHasOccured = false;
        if (PlayerModeSwitch.ImInNormalMode) {
            TP_RB = sources.ThelosPlaya.GetComponent<Rigidbody>();
            ThelosNorModeVel[0] = TP_RB.velocity.x;
            ThelosNorModeVel[1] = TP_RB.velocity.y;
            ThelosNorModeVel[2] = TP_RB.velocity.z;
        }
    }
    private void Update()
    {
        //----------------------------------------------------------------
        Seals SL = sources.ThelosPlaya.GetComponent<Seals>();
        RedSealObj = SL.RedSeal;BlueSealObj = SL.BlueSeal;
SJ_Count = MySpecjump.SpecJumpCount;
        KeepISPlayaInNormalMode = PlayerModeSwitch.ImInNormalMode;
        KeepRecVal = RecoveryBar.TheRecoveryVar;
        if (PlayerModeSwitch.ImInNormalMode)
        {
            Vector3 HoldPos = sources.ThelosPlaya.transform.position;
            HoldPos.y = HoldPos.y - sources.MovingOrigin.transform.position.y;
            PlayaPos[0] = HoldPos.x;
            PlayaPos[1] = HoldPos.y;
            PlayaPos[2] = HoldPos.z;
        }
        else
        {
            GameObject spine = PlayaMoveRigidB.ThelosAnimator.transform.GetChild(0).transform.GetChild(0).gameObject;
            Vector3 HoldPos = spine.transform.position;
            HoldPos.y = HoldPos.y - sources.MovingOrigin.transform.position.y;
            PlayaPos[0] = HoldPos.x;
            PlayaPos[1] = HoldPos.y;
            PlayaPos[2] = HoldPos.z;
        }
        //------------------------------------------------------------------------------------
       RedSealPos[0] =Seals.LastRedPos.x; BlueSealPos[0] = Seals.LastBluePos.x;
        RedSealPos[1] = Seals.LastRedPos.y; BlueSealPos[1] = Seals.LastBluePos.y;
        RedSealPos[2] = Seals.LastRedPos.z; BlueSealPos[2] = Seals.LastBluePos.z;
    }
    void Start()
    {
    }
    public void SaveSys()
    {
      //  print("SavedPlayapos:   " + PlayaPos[0] + "_" + PlayaPos[1] + "_" + PlayaPos[2]);
        SaveSenRots();
        if (SentryConnection.RemSennTime != 0)
        {
            if(SaveString == "/savedata.ffua")
            {
                PlayerPrefs.SetFloat("KeepSennTime_slot1", SentryConnection.RemSennTime);
                PlayerPrefs.SetInt("MustLoadSennTime_slot1", 1);
            }
            else
            {
                PlayerPrefs.SetFloat("KeepSennTime_slot2", SentryConnection.RemSennTime);
                PlayerPrefs.SetInt("MustLoadSennTime_slot2", 1);
            }
        }
        else
        {
            if (SaveString == "/savedata.ffua") PlayerPrefs.SetFloat("KeepSennTime_slot1", 0);
            if (SaveString == "/savedataSecondary.ffua") PlayerPrefs.SetFloat("KeepSennTime_slot2", 0);
        }
        AssignTheAnimSaveInfo();
        SaveSystem.SavePlayer(this);
    }
    public void LoadSys()
    {
        if (SaveString == "/savedata.ffua") PlayerPrefs.SetInt("MustLoadSennTime_slot1", 1);
        if (SaveString == "/savedataSecondary.ffua") PlayerPrefs.SetInt("MustLoadSennTime_slot2", 1);
        ALoadHasOccured = true;
        LoadSenRots();
        Seals SL = sources.ThelosPlaya.GetComponent<Seals>();
        RedSealObj = SL.RedSeal; BlueSealObj = SL.BlueSeal;
        PlayerData Data = SaveSystem.LoadPlayer();
        //print("Loaded Playa pos:   " + Data.PlayaPos[0] + "_" + Data.PlayaPos[1] + "_" + Data.PlayaPos[2]);
        MySpecjump.SpecJumpCount = Data.KeepSJ_Count;
        RecoveryBar.TheRecoveryVar = Data.KeepRecVal;
        RedSealObj.transform.parent = null;
        RedSealObj.transform.position = new Vector3(Data.KeepRedSealPos[0], Data.KeepRedSealPos[1], Data.KeepRedSealPos[2]);
        BlueSealObj.transform.parent = null;
        BlueSealObj.transform.position = new Vector3(Data.KeepBlueSealPos[0], Data.KeepBlueSealPos[1], Data.KeepBlueSealPos[2]);
        if (Data.KeepISPlayaInNormalMode)
        {
            if (PlayerModeSwitch.ImInNormalMode)
            {
                PlayaMoveRigidB.ThelosAnimator.Play(Data.KeepStateName, -1, Data.KeepStateProgress);
                sources.ThelosPlaya.GetComponent<Rigidbody>().velocity = new Vector3(Data.KeepThelosVelocity[0], Data.KeepThelosVelocity[1], Data.KeepThelosVelocity[2]);
                Vector3 PlayaPos = new Vector3(Data.PlayaPos[0], Data.PlayaPos[1], Data.PlayaPos[2]);
                sources.ThelosPlaya.transform.position = new Vector3(Data.PlayaPos[0], Data.PlayaPos[1], Data.PlayaPos[2]);
            }
            else
            {
                GameObject RootObj = sources.srcsINT.Spine03.transform.root.gameObject;
                Rigidbody[] AllRBcomponents = RootObj.GetComponentsInChildren<Rigidbody>();
                for (int i = 0; i < AllRBcomponents.Length; i++)
                {
                    if (AllRBcomponents[i].isKinematic == false)
                    {
                        AllRBcomponents[i].velocity = Vector3.zero;
                    }
                }
                PlayerModeSwitch.StateOfPlayaMode = -1;
                StartCoroutine(RePos(new Vector3(Data.PlayaPos[0], Data.PlayaPos[1], Data.PlayaPos[2])));
            }
        }
        else
        {
            GameObject spine = PlayaMoveRigidB.ThelosAnimator.transform.GetChild(0).transform.GetChild(0).gameObject;
            if (PlayerModeSwitch.ImInNormalMode)
            {
                RecoveryBar.TheRecoveryVar = 0;
                StartCoroutine(RePos2(new Vector3(Data.PlayaPos[0], Data.PlayaPos[1], Data.PlayaPos[2])));
            }
            else
            {
                GameObject RootObj = sources.srcsINT.Spine03.transform.root.gameObject;
                Rigidbody[] AllRBcomponents = RootObj.GetComponentsInChildren<Rigidbody>();
                for (int i = 0; i < AllRBcomponents.Length; i++)
                {
                    if (AllRBcomponents[i].isKinematic == false)
                    {
                        AllRBcomponents[i].velocity=Vector3.zero;
                    }
                }
                spine.transform.position = new Vector3(Data.PlayaPos[0], Data.PlayaPos[1], Data.PlayaPos[2]);
            }
        }
        Time.timeScale = 0;
        PlayerPrefs.SetInt("ReLoadSlot1FromMain", 0);
        PlayerPrefs.SetInt("ReLoadSlot2FromMain", 0);
    }
    IEnumerator ReloadSlot1()
    {
        yield return new WaitForSeconds(0.1f);
        SetStringForSlot1();
        LoadSys();
       // PlayerPrefs.SetInt("ReLoadSlot1FromMain", 0);
    }
    IEnumerator ReloadSlot2()
    {
        yield return new WaitForSeconds(0.1f);
        SetStringForSlot2();
        LoadSys();
       // PlayerPrefs.SetInt("ReLoadSlot2FromMain", 0);
    }
    IEnumerator RePos(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        sources.ThelosPlaya.transform.position = pos;
    }
    IEnumerator RePos2(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject spine = PlayaMoveRigidB.ThelosAnimator.transform.GetChild(0).transform.GetChild(0).gameObject;
        spine.transform.position = pos;
    }
    public void SetStringForSlot1()
    {
        SaveString = "/savedata.ffua";
        PlayerPrefs.SetString("ASlot1ExistsNow",SaveString);
    }
    public void SetStringForSlot2()
    {
        SaveString = "/savedataSecondary.ffua";
        PlayerPrefs.SetString("ASlot2ExistsNow", SaveString);
    }
    void AssignTheAnimSaveInfo()
    {
        Animator anim = PlayaMoveRigidB.ThelosAnimator;

        AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;

        // Iterate over the clips and gather their information
        foreach (AnimationClip animClip in animationClips)
        {

            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animClip.name))
            {
                HoldStateName = animClip.name;
                HoldStateProgress = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
        }
    }
    void SaveSenRots()
    {
        GameObject[] AllSenObjs = GameObject.FindGameObjectsWithTag("ASentry_OBJ");
        int index = 1;
        if (SaveString == "/savedataSecondary.ffua") index = 2;
        for (int i = 0; i < AllSenObjs.Length; i++)
        {
            AllSenObjs[i].GetComponent<SentryConnection>().SaveRots(index);
        }
    }
    void LoadSenRots()
    {
        GameObject[] AllSenObjs = GameObject.FindGameObjectsWithTag("ASentry_OBJ");
        int index = 1;
        if (SaveString == "/savedataSecondary.ffua") index = 2;
        for (int i = 0; i < AllSenObjs.Length; i++)
        {
            AllSenObjs[i].GetComponent<SentryConnection>().LoadRots(index);
        }
    }
}
