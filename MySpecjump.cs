using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySpecjump : MonoBehaviour
{
    Rigidbody RB;
    public static bool DoingSpecJump;
    public GameObject ThelosObj;
    public float JumpSpecPow;
    Animator ThelosAnim;
    public ParticleSystem SpinEff_L;
    public ParticleSystem SpinEff_R;
    public ParticleSystem SpiralPS;
    bool flag; bool CastJump;
    float startTime;
    public bool ImInUltraMode;
    public static int SpecJumpCount;
    public static int MySpecjumpState;//should be -1,0,1 or 2
    // Start is called before the first frame update
    private bool SpecJumpFlag;
    private float UltraTimer;
    public ParticleSystem UltraPS;
    private bool MyFlagForRot;
    public static Quaternion LastCorrThelosRot;
    Vector3 VecVel;
    private bool AFlag;
    private bool SJ_CD_ISON;
    private bool Spiny_flag;
    private float SealIdleTime;
    public int Starting_SJ_COUNT;public ParticleSystem SJ_cir_PS;
    void Start()
    {
        SealIdleTime = Time.time;
        Spiny_flag = true;
        SJ_CD_ISON = false;
        AFlag = false;
        VecVel = transform.position;
        MyFlagForRot = false;
        UltraTimer = 0;
        ImInUltraMode = false;
        SpecJumpFlag = false;
        SpecJumpCount = Starting_SJ_COUNT;
        CastJump = true;
        flag = true;
        DoingSpecJump = false;
        RB = GetComponent<Rigidbody>();
        ThelosAnim = ThelosObj.GetComponent<Animator>();
        SpinEff_L.Play();
        SpinEff_R.Play();
        var Eff_L_Emiss = SpinEff_L.emission;
        var Eff_R_Emiss = SpinEff_R.emission;
        Eff_L_Emiss.rateOverDistance = 0;
        Eff_R_Emiss.rateOverDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SpinUltiJump")&& !ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("MidAirJump"))
        {
            SpiralPS.Stop();
        }
        if (!ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SpinUltiJump") && !ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("MidAirJump") && !ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SJPointer"))
        {
            if (PlayerModeSwitch.ImInNormalMode)
            {
                ThelosObj.transform.Rotate(Vector3.zero);
            }
        }
        if (UnderWater.PlayaIsUnderWater) DoingSpecJump = false;
        GeneralAudioClips.GAC.Num26.volume -= 8 * Time.deltaTime;
        if (SpecJumpCount < 1) MySpecjumpState = -1; else MySpecjumpState = 0;
        if (UnderWater.PlayaIsUnderWater == false && PlayerModeSwitch.ImInNormalMode && PlayerModeSwitch.ImInRecoveryMode == false && SpecJumpCount > 0 && PlayaMoveRigidB.ThelosAnimator.GetFloat("SJP_Speed")==1)
        {
            MyFlagForRot = false;
            var Eff_L_Emiss = SpinEff_L.emission;
            var Eff_R_Emiss = SpinEff_R.emission;
            if (Input.GetKeyDown(KeyCode.Tab) && ImInUltraMode && PlayaMoveRigidB.PlayaIsGrounded == false && !RestrictionSJ.RestrictedSJ)
            {
                if (!GeneralAudioClips.GAC.Num27.isPlaying) GeneralAudioClips.GAC.Num27.Play();
                GameObject.Find("SpecHolderUI").GetComponent<SpecJumpUI>().GOGO();
                SpecJumpCount -= 1;
                RB.isKinematic = false;
                ImInUltraMode = true;
                UltraTimer = Time.time;
                SpecJumpFlag = true;
                SJ_cir_PS.Play();
                PlayaMoveRigidB.Flag_DontRepeatWingPop = true;
                UpdateBrightness.SJ_Blur = true; UpdateBrightness.SpaceCurvePow = 65.0f; UpdateBrightness.WideCamAngle_flag = true;
                ThelosAnim.Play("SJPointer", -1, 0f);
                sources.srcsINT.GlobVolObj.GetComponent<UpdateBrightness>().SetVisualDash(1.0f);
                RB.velocity = Vector3.zero;
                GeneralAudioClips.GAC.Num27.Play();
            }
            //-------------------------------------------------------------------------------------
            if (!(ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SpinUltiJump") && Input.GetKey(KeyCode.Tab))){
                GeneralAudioClips.GAC.Num25.volume -= 4 * Time.deltaTime;
                Eff_L_Emiss.rateOverDistance = 0; Eff_R_Emiss.rateOverDistance = 0;
                SpinEff_L.Clear(); SpinEff_R.Clear();
            }
            if (ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("MidAirJump"))
            {
                MySpecjumpState = 1;
                SpiralPS.Play();
                if (flag) startTime = Time.time;
                if (Time.time - startTime >= 3) CancelTheJump();
                flag = false;
                GeneralAudioClips.GAC.Num26.volume = 1;
                if (!GeneralAudioClips.GAC.Num26.isPlaying) GeneralAudioClips.GAC.Num26.Play();
            }
            else
            {
                SpiralPS.Stop();
                flag = true;
            }
            if (PlayerModeSwitch.ImInNormalMode == false) SpiralPS.Stop();
            //------------------------------------------------------------------
            if (!ImInUltraMode)
            {
                if (Input.GetKeyUp(KeyCode.Tab) && ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SpinUltiJump"))
                {
                    //ThelosAnim.Play("Jump", -1, 0f);
                    //ThelosAnim.transform.rotation = LastCorrThelosRot;
                    // CastJump = false;
                    CancelTheJump();
                }
                {
                    if (Input.GetKeyUp(KeyCode.Tab) && ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("MidAirJump"))
                    {
                        if (!GeneralAudioClips.GAC.Num27.isPlaying) GeneralAudioClips.GAC.Num27.Play();
                        GameObject.Find("SpecHolderUI").GetComponent<SpecJumpUI>().GOGO();
                        SpecJumpCount -= 1;
                        RB.isKinematic = false;
                        ImInUltraMode = true;
                        UltraTimer = Time.time;
                        SpecJumpFlag = true;
                        SJ_cir_PS.Play();
                        PlayaMoveRigidB.Flag_DontRepeatWingPop = true;
                        UpdateBrightness.SJ_Blur = true; UpdateBrightness.SpaceCurvePow = 65.0f; UpdateBrightness.WideCamAngle_flag = true;
                        sources.srcsINT.GlobVolObj.GetComponent<UpdateBrightness>().SetVisualDash(1.0f);
                    }
                }
                if (ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SpinUltiJump") && Input.GetKey(KeyCode.Tab))
                {
                    MySpecjumpState = 1;
                    Eff_L_Emiss.rateOverDistance = 20;
                    Eff_R_Emiss.rateOverDistance = 20;
                    GeneralAudioClips.GAC.Num25.volume = 0.7f;
                    if (!GeneralAudioClips.GAC.Num25.isPlaying) GeneralAudioClips.GAC.Num25.Play();
                }
                else
                {
                    GeneralAudioClips.GAC.Num25.volume -= 4 * Time.deltaTime;
                    Eff_L_Emiss.rateOverDistance = 0; Eff_R_Emiss.rateOverDistance = 0;
                    SpinEff_L.Clear(); SpinEff_R.Clear();
                }
                if (PlayerModeSwitch.ImInNormalMode == false) SpiralPS.Stop();
                if (Input.GetKeyDown(KeyCode.Tab) && !SJ_CD_ISON) { CastJump = true; StartCoroutine(ForTHeCD()); }
                if (Input.GetKey(KeyCode.Tab) && PlayaMoveRigidB.PlayaIsGrounded == false && CastJump && RestrictionSJ.RestrictedSJ == false)
                {
                    RB.isKinematic = true;
                    DoingSpecJump = true;
                    Vector3 LookPos = transform.position + Camera.main.transform.forward;
                    ThelosObj.transform.LookAt(LookPos);
                    ThelosObj.transform.Rotate(90, 0, 0);
                    MyFlagForRot = true;
                    if (ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
                    {
                        ThelosAnim.SetInteger("AnimPar", 1);
                        CastJump = false;
                    }
                }
                else
                {
                   // MyFlagForRot = false;
                    RB.isKinematic = false;
                    DoingSpecJump = false;
                }
                if (Input.GetKey(KeyCode.Tab) && PlayaMoveRigidB.PlayaIsGrounded == false && CastJump && RestrictionSJ.RestrictedSJ)
                {
                    LeanTween.scale(GameObject.Find("ReddoImg"), new Vector3(1.2f, 1.2f, 1.2f), 0.1f).setOnComplete(Rescale);
                }
            }
            else { SpiralPS.Stop(); }
        }
        //---------------------------------------------------------------------------------------------------
        if (ImInUltraMode)
        {
            if (UltraPS.isPlaying == false) UltraPS.Play();
            if (Time.time - UltraTimer >= 2) ImInUltraMode = false;
        }
        else UltraPS.Clear();
            if (ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SJPointer")|| ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("MidAirJump")&& !MyFlagForRot)
        {
            if (PlayerModeSwitch.ImInNormalMode && !AFlag)
            {
                Vector3 VelDir = Vector3.Normalize(RB.velocity);
                if (ThelosAnim.GetFloat("SJP_Speed") > 2) VelDir.y = 0.5f;
                Vector3 LookPos = transform.position + VelDir * 5;
                ThelosObj.transform.LookAt(LookPos);
               ThelosObj.transform.Rotate(90, 0, 0);
            }
        }
        AFlag = false;
        if (PlayerModeSwitch.ImInNormalMode == false)
        {
            SpiralPS.Stop();
            var Eff_L_Emiss = SpinEff_L.emission;
            var Eff_R_Emiss = SpinEff_R.emission;
            Eff_L_Emiss.rateOverDistance = 0; Eff_R_Emiss.rateOverDistance = 0;
            SpinEff_L.Clear(); SpinEff_R.Clear();
        }
        if (PlayaMoveRigidB.ThelosAnimator.GetCurrentAnimatorStateInfo(0).IsName("SealIdle") || PlayaMoveRigidB.ThelosAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spiny"))
        {
            if (PlayaMoveRigidB.PlayaActualMoveSpeed > 5)
            {
                Vector3 VelDir = Vector3.Normalize(RB.velocity);
                Vector3 LookPos = transform.position + VelDir * 5;
                // ThelosObj.transform.LookAt(LookPos);
                //  ThelosObj.transform.Rotate(90, 0, 0);
                // Quaternion lookOnLook = Quaternion.LookRotation(LookPos - transform.position,transform.forward);
                Quaternion lookOnLook = Quaternion.LookRotation(LookPos - transform.position, transform.forward);
                //Vector3 vec = lookOnLook.eulerAngles;
                //vec.x += 90;
                // lookOnLook = Quaternion.Euler(vec);
                ThelosObj.transform.rotation = lookOnLook;
                ThelosObj.transform.Rotate(90, 0, 0);
            }
            else
            {
                Vector3 Rot = ThelosObj.transform.rotation.eulerAngles;
                Rot.x = 0; Rot.z = 0;
                ThelosObj.transform.rotation = Quaternion.Euler(Rot);
            }
        }
    }
    private void FixedUpdate()
    {
        if (SpecJumpFlag)
        {
            Vector3 Dir = Camera.main.transform.forward;
            Dir = Dir * JumpSpecPow;
            RB.AddForce(Dir, ForceMode.VelocityChange);
            SpecJumpFlag = false;
        }
    }
    public void CancelTheJump()
    {
        CastJump = false;
        DoingSpecJump = false;
        RB.isKinematic = false;
        ThelosAnim.Play("Jump", -1, 0f);
        ThelosAnim.transform.rotation = LastCorrThelosRot;
        AFlag = true;
    }
    public void Rescale()
    {
        LeanTween.scale(GameObject.Find("ReddoImg"), new Vector3(1.75f, 1.75f, 1.75f), 0.1f);
    }
    private void LateUpdate()
    {
        if (!PlayerModeSwitch.ImInRecoveryMode)
        {
            if ( ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SJPointer") || ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("MidAirJump") && !MyFlagForRot)
            {
                if (PlayerModeSwitch.ImInNormalMode)
                {

                    Vector3 VelDir = Vector3.Normalize(RB.velocity);
                    if (ThelosAnim.GetFloat("SJP_Speed") > 2) VelDir.y = 0.5f;
                    Vector3 LookPos = transform.position + VelDir * 5;
                     ThelosObj.transform.LookAt(LookPos);
                     ThelosObj.transform.Rotate(90, 0, 0);
                }
            }
        }
        else {

            ThelosAnim.transform.rotation = LastCorrThelosRot;
        }
        if (PlayaMoveRigidB.PlayaIsGrounded)
        {
            ThelosAnim.transform.rotation = LastCorrThelosRot;
        }
        if (ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") || ThelosAnim.GetCurrentAnimatorStateInfo(0).IsName("SecJump"))
        {
            Vector3 Rot = ThelosObj.transform.rotation.eulerAngles;
            Rot.x = 0;Rot.z = 0;
            ThelosObj.transform.rotation = Quaternion.Euler(Rot); 
        }
        ThelosAnim.SetBool("UCan_add_Spin", false);
        if (PlayaMoveRigidB.ThelosAnimator.GetCurrentAnimatorStateInfo(0).IsName("SealIdle"))
        {
            if (Spiny_flag) { SealIdleTime = Time.time; Spiny_flag = false; }
            if (Time.time - SealIdleTime >= 0.6f && PlayaMoveRigidB.PlayaActualMoveSpeed>25)
            {
                ThelosAnim.SetBool("UCan_add_Spin",true);
            }
        }
        else Spiny_flag = true;

    }
    IEnumerator ForTHeCD()
    {
        SJ_CD_ISON = true;
        yield return new WaitForSeconds(0.6f);
        SJ_CD_ISON = false;
    }
    public void ForReset()
    {
        ImInUltraMode = true;
        UltraTimer = Time.time;
    }
}
