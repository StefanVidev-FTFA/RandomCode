using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class sources : MonoBehaviour {
	// Use this for initialization
	public static sources srcsINT;
	public  GameObject Spine03;
	public  GameObject CinemaOBJ;
	public CinemachineVirtualCamera MyCin;
	CinemachineComponentBase componentBase;
	public static GameObject MyImplosion;
	public static GameObject ThelosPlaya;
	public static GameObject UnMovingOrigin;
	public static GameObject MovingOrigin;
	public ParticleSystem Spawn_PS;
	public GameObject GlobVolObj;
	private void Awake()
    {
		MovingOrigin = GameObject.Find("MovingOrigin_DONTDELME");
		UnMovingOrigin = GameObject.Find("UnMovingOrigin_DONTDELME");
		ThelosPlaya = gameObject;
		MyImplosion = GameObject.Find("Implosion_DONTDELME").gameObject;
		srcsINT = transform.GetComponent<sources>();
    }
    void Start () {
		MyCin = CinemaOBJ.GetComponent<CinemachineVirtualCamera>();
		componentBase = MyCin.GetCinemachineComponent(CinemachineCore.Stage.Body);
	}
	// Update is called once per frame
	void Update () {
	}
	public void InvokeCamShake()
    {
		CinemaOBJ.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
	}
	public float Distance2(GameObject obj1, GameObject obj2){
		float x1 = 0.0f;
		float y1 = 0.0f;
		float z1 = 0.0f;
		if (obj1.transform.position.x > obj2.transform.position.x) {
			x1 = obj1.transform.position.x - obj2.transform.position.x;
		} else
			x1 = obj2.transform.position.x - obj1.transform.position.x;
		//-----------------------------------------------------------
		if (obj1.transform.position.y > obj2.transform.position.y) {
			y1 = obj1.transform.position.y - obj2.transform.position.y;
		} else
			y1 = obj2.transform.position.y - obj1.transform.position.y;
		//-----------------------------------------------------------
		if (obj1.transform.position.z > obj2.transform.position.z) {
			z1 = obj1.transform.position.z - obj2.transform.position.z;
		} else
			z1 = obj2.transform.position.z - obj1.transform.position.z;
		//-----------------------------------------------------------
		float firstTri = Mathf.Sqrt(x1*x1+z1*z1);
		firstTri = Mathf.Sqrt (firstTri * firstTri + y1 * y1);
		return firstTri;
	}
	public float Distance2wV3(Vector3 VectorPos1, Vector3 VectorPos2){
		float x1 = 0.0f;
		float y1 = 0.0f;
		float z1 = 0.0f;
		if (VectorPos1.x > VectorPos2.x) {
			x1 = VectorPos1.x - VectorPos2.x;
		} else
			x1 = VectorPos2.x - VectorPos1.x;
		//-----------------------------------------------------------
		if (VectorPos1.y > VectorPos2.y) {
			y1 = VectorPos1.y - VectorPos2.y;
		} else
			y1 = VectorPos2.y - VectorPos1.y;
		//-----------------------------------------------------------
		if (VectorPos1.z > VectorPos2.z) {
			z1 = VectorPos1.z - VectorPos2.z;
		} else
			z1 = VectorPos2.z - VectorPos1.z;
		//-----------------------------------------------------------
		float firstTri = Mathf.Sqrt(x1*x1+z1*z1);
		firstTri = Mathf.Sqrt (firstTri * firstTri + y1 * y1);
		return firstTri;
	}
	public Vector3 NewYCirclePos(Vector3 centerPos,float angle,float theradius)
	{
		float deg = 360-angle;
		angle = deg*Mathf.PI/180;
		float myX = Mathf.Sin(angle)*theradius;
		float myZ = Mathf.Cos(angle)*theradius;
		Vector3 pos = new Vector3(centerPos.x+myX,centerPos.y,centerPos.z+myZ);
		return pos;
	}
	public float GetAngle3Pts(Vector3 FirstPt,Vector3 MidPt,Vector3 EndPt){
		float angle = Vector2.Angle (FirstPt - MidPt, EndPt - MidPt);
		return angle;
	}
	public Vector3 GetCirclePos(Vector3 A,Vector3 C,Vector3 B,float step){
		// this function receives vector3 of 3 points in 3D space
		// A and B are the start and end and the C is the between point
		//where an angle occurs, this function takes the step and adds it to A
		//in such a way so taht it creates a circular motion moving towards B
		// we consider the radius to be the distance between A and C
		float r = Distance2wV3 (A, C);
		float CurrBCDist = Distance2wV3(B,C);
		float HoldPerc = r / CurrBCDist;
		float HDirX = B.x-C.x;float HDirY = B.y-C.y;float HDirZ = B.z-C.z;
		HDirX = HDirX * HoldPerc;HDirY = HDirY * HoldPerc;HDirZ = HDirZ * HoldPerc;
		Vector3 NewPos = Vector3.zero;
		NewPos.x = C.x + HDirX;
		NewPos.y = C.y + HDirY;
		NewPos.z = C.z + HDirZ;
		B = NewPos;
		NewPos = Vector3.zero;
		float DirX = B.x-A.x;float DirY = B.y-A.y;float DirZ = B.z-A.z;
		float ABDist = Distance2wV3 (A, B);
		float Perc = step / ABDist;
		DirX = DirX * Perc;DirY = DirY * Perc;DirZ = DirZ * Perc;
		NewPos.x = A.x + DirX;
		NewPos.y = A.y + DirY;
		NewPos.z = A.z + DirZ;
		float NewPtoRDist = Distance2wV3 (C, NewPos);
		Perc = r / NewPtoRDist;
		DirX = NewPos.x-C.x; DirY = NewPos.y-C.y; DirZ = NewPos.z-C.z;
		DirX = DirX * Perc;DirY = DirY * Perc;DirZ = DirZ * Perc;
		NewPos.x = C.x + DirX;
		NewPos.y = C.y + DirY;
		NewPos.z = C.z + DirZ;
		return NewPos;
	}
	public Vector3 UpdateVec3(Vector3 TheVec,float Xadd,float Yadd,float Zadd){
		Vector3 NewVec = Vector3.zero;
		NewVec = new Vector3 (TheVec.x + Xadd, TheVec.y + Yadd, TheVec.z + Zadd);
		return NewVec;
	}
	public Vector3 VecDir(Vector3 Vec1, Vector3 Vec2){
		// vec 2 is the dir of the vector
		Vector3 NewDir = Vector3.zero;
		NewDir.x = Vec2.x - Vec1.x;NewDir.y = Vec2.y - Vec1.y;NewDir.z = Vec2.z - Vec1.z;
		return NewDir;
	}
	public Vector3 SumVec3(Vector3 Vec1,Vector3 Vec2){
		// adds vec2 to vec1, so it updates vec1
		Vector3 NewVec=Vector3.zero;
		NewVec.x = Vec1.x + Vec2.x;NewVec.y = Vec1.y + Vec2.y;NewVec.z = Vec1.z + Vec2.z;
		return NewVec;
	}
	public Vector3 Vec3Move(Vector3 Vec1,Vector3 Vec2, float step){
		// this function returns a new pos of size step in the dir ot Vec1 towards Vec2
		Vector3 NewPos;
		float ABDist = Distance2wV3(Vec1,Vec2);
		float perc = step / ABDist;
		if (perc > 0) {
			NewPos = VecDir (Vec1, Vec2);

			NewPos.x = Vec1.x + perc * NewPos.x;NewPos.y = Vec1.y + perc * NewPos.y;NewPos.z = Vec1.z + perc * NewPos.z;
		} else
			NewPos = Vec1;
		return NewPos;
	}
	public void Turn_Off_or_ON_All_RB_InthisGO(GameObject TheParentObj,bool OnOrOff)// if bool is false then turn the off, and vice-versa
    {
		Rigidbody[] AllRBcomponents = TheParentObj.GetComponentsInChildren<Rigidbody>();
		for(int i=0;i< AllRBcomponents.Length; i++)
        {
			if (AllRBcomponents[i].name != "ThelosPlayer")
			{
				if (OnOrOff == false)
				{
					AllRBcomponents[i].isKinematic = true;
				}
				else { AllRBcomponents[i].isKinematic = false; AllRBcomponents[i].drag = 1; }
			}
			}
		Collider[] All_Collider_components = TheParentObj.GetComponentsInChildren<Collider>();
		for (int i = 0; i < All_Collider_components.Length; i++)
		{
				if (OnOrOff == false)
				{
					All_Collider_components[i].enabled = false;
				}
				else All_Collider_components[i].enabled = true;
			if (All_Collider_components[i].name == "StepAudColl")
            {
				if (OnOrOff)
				All_Collider_components[i].enabled = false;
				else All_Collider_components[i].enabled = true;
			}

		}
	}
	public Vector3[] ArrPushV3New(Vector3[] arr, Vector3 Element)
	{
		Vector3[] NewArr;
		if (arr == null)
		{
			NewArr = new Vector3[1];
			NewArr[0] = Element;
		}
		else
		{
			int flag = 0;
			NewArr = new Vector3[arr.Length + 1];
			for (int i = 0; i < arr.Length; i++)
			{
				if (Element == arr[i])
				{
					flag = 1;
				}
			}
			if (flag == 0)
			{
				NewArr = new Vector3[arr.Length + 1];
				for (int i = 0; i < arr.Length; i++)
				{
					NewArr[i] = arr[i];
				}
				NewArr[arr.Length] = Element;
			}
			else
				NewArr = arr;
		}
		return NewArr;
	}
	public Vector3[] ArrPushV3(Vector3[] arr, Vector3 Element)
	{
		Vector3[] NewArr;
		if (arr == null)
		{
			NewArr = new Vector3[1];
			NewArr[0] = Element;
		}
		else
		{
				NewArr = new Vector3[arr.Length + 1];
				for (int i = 0; i < arr.Length; i++)
				{
					NewArr[i] = arr[i];
				}
				NewArr[arr.Length] = Element;
		}
		return NewArr;
	}
	public string[] ArrPushString(string[] arr, string Element)
	{
		string[] NewArr;
		if (arr == null)
		{
			NewArr = new string[1];
			NewArr[0] = Element;
		}
		else
		{
			NewArr = new string[arr.Length + 1];
			for (int i = 0; i < arr.Length; i++)
			{
				NewArr[i] = arr[i];
			}
			NewArr[arr.Length] = Element;
		}
		return NewArr;
	}
	public GameObject[] ArrPush_GO(GameObject[] arr, GameObject Element)
	{
		GameObject[] NewArr;
		if (arr == null)
		{
			NewArr = new GameObject[1];
			NewArr[0] = Element;
		}
		else
		{
			NewArr = new GameObject[arr.Length + 1];
			for (int i = 0; i < arr.Length; i++)
			{
				NewArr[i] = arr[i];
			}
			NewArr[arr.Length] = Element;
		}
		return NewArr;
	}
	public Vector3[] ArrPushV3Front(Vector3[] arr, Vector3 Element)
	{
		Vector3[] NewArr;
		if (arr == null)
		{
			NewArr = new Vector3[1];
			NewArr[0] = Element;
		}
		else
		{
				NewArr = new Vector3[arr.Length + 1];
			NewArr[0] = Element;
				for (int i = 1; i < arr.Length+1; i++)
				{
					NewArr[i] = arr[i-1];
				}
		}
		return NewArr;
	}
	public GameObject[] ArrPop(GameObject[] arr)
	{
		GameObject[] NewArr;
		if (arr == null)
		{
			NewArr = arr;
		}
		else
		{
			NewArr = new GameObject[arr.Length - 1];
			for (int i = 0; i < arr.Length - 1; i++)
			{
				NewArr[i] = arr[i];
			}
		}
		return NewArr;
	}
	public Vector3[] ArrPop_YoungestV3(Vector3[] arr)
	{
		Vector3[] NewArr;
		if (arr == null)
		{
			NewArr = arr;
		}
		else
		{
			NewArr = new Vector3[arr.Length - 1];
			for (int i = 1; i < arr.Length; i++)
			{
				NewArr[i-1] = arr[i];
			}
		}
		return NewArr;
	}
	public GameObject[] ArrPop_Youngest_GO(GameObject[] arr)
	{
		GameObject[] NewArr;
		if (arr == null)
		{
			NewArr = arr;
		}
		else
		{
			NewArr = new GameObject[arr.Length - 1];
			for (int i = 1; i < arr.Length; i++)
			{
				NewArr[i - 1] = arr[i];
			}
		}
		return NewArr;
	}
	public void RotateTowardsPos(Transform OBJtoRotate,Vector3 ObjPos,Vector3 TargetObjPos,float LerpVal)
    {
		Vector3 targetROT = TargetObjPos - ObjPos;
		Quaternion Rot = Quaternion.LookRotation(targetROT);
		if(Rot.eulerAngles!=Vector3.zero)	OBJtoRotate.rotation = Quaternion.Lerp(OBJtoRotate.rotation, Rot, LerpVal * Time.deltaTime);
		//------------- HOW TO USE BELOW -------------
		//        sources sc = sources.srcsINT;
		//sc.RotateTowardsPos(transform, transform.position, Target.transform.position, LerpVal);
	}
	public void ChangeCamFollandLookTarget(GameObject FollTarget,GameObject LookTarget)
    {
		MyCin.Follow = FollTarget.transform;
		MyCin.LookAt = LookTarget.transform;
	}
	public void ChangeCamOffset(Vector3 NewOffset)
    {
		if (componentBase is CinemachineTransposer)
		{
			(componentBase as CinemachineTransposer).m_FollowOffset = NewOffset; // your value
		}
	}
	public void ChangeCamFieldView(float Angle)
    {
		MyCin.m_Lens.FieldOfView = Angle;
	}
	public void ChangeCamFarClip(float distance)
	{
		MyCin.m_Lens.FarClipPlane = distance;
	}
	public Vector3[] CopyArrV3(Vector3[] arr)
    {
		Vector3[] NewArr = new Vector3[arr.Length];
		for(int i = 0; i < arr.Length; i++)
        {
			NewArr[i] = arr[i];
        }
		return NewArr;
    }
	public float[] BubleSortArray(float[] array)
	{

		float temp = array[0];

		for (int i = 0; i < array.Length; i++)
		{
			for (int j = i + 1; j < array.Length; j++)
			{
				if (array[i] > array[j])
				{
					temp = array[i];

					array[i] = array[j];

					array[j] = temp;
				}
			}
		}
		return array;
	}
	public float ForTheWaveTriBubleSort(Vector3[] verts,Vector3 CamsPos)
    {
			CamsPos.y = 0;
		Vector3[] NullYVerts = CopyArrV3(verts);
		for (int i = 0; i < NullYVerts.Length; i++)
        {
			NullYVerts[i].y = 0;
		}
		//----------------------------------
		Vector3 temp = Vector3.zero;
		Vector3 temp2 = Vector3.zero;

		for (int i = 0; i < verts.Length; i++)
		{
			for (int j = i + 1; j < verts.Length; j++)
			{
				if (Vector3.Distance(NullYVerts[i], CamsPos) > Vector3.Distance(NullYVerts[j], CamsPos))
				{
					temp = verts[i];

					verts[i] = verts[j];

					verts[j] = temp;
					//--------------------------
					temp2 = NullYVerts[i];

					NullYVerts[i] = NullYVerts[j];

					NullYVerts[j] = temp2;
				}
			}
		}
		Vector3 TwoKeyYs = Vector3.zero;
		TwoKeyYs.x = verts[0].y;
		TwoKeyYs.y = verts[1].y;
		TwoKeyYs.z = Vector2.Distance(new Vector2(verts[0].z, verts[0].x), new Vector2(CamsPos.x, CamsPos.z)) / Vector2.Distance(new Vector2(verts[1].z, verts[1].x), new Vector2(CamsPos.x, CamsPos.z));
		if (Vector2.Distance(new Vector2(verts[2].z, verts[2].x), new Vector2(CamsPos.x, CamsPos.z)) < Vector2.Distance(new Vector2(verts[1].z, verts[1].x), new Vector2(CamsPos.x, CamsPos.z)))
		{
			TwoKeyYs.y = verts[2].y;
			TwoKeyYs.z = Vector2.Distance(new Vector2(verts[0].z, verts[0].x), new Vector2(CamsPos.x, CamsPos.z)) / Vector2.Distance(new Vector2(verts[2].z, verts[2].x), new Vector2(CamsPos.x, CamsPos.z));
			if (Vector2.Distance(new Vector2(verts[2].z, verts[2].x), new Vector2(CamsPos.x, CamsPos.z)) < Vector2.Distance(new Vector2(verts[0].z, verts[0].x), new Vector2(CamsPos.x, CamsPos.z))){
				TwoKeyYs.x = verts[2].y;
				TwoKeyYs.y = verts[0].y;
				TwoKeyYs.z = Vector2.Distance(new Vector2(verts[2].z, verts[2].x), new Vector2(CamsPos.x, CamsPos.z)) / Vector2.Distance(new Vector2(verts[0].z, verts[0].x), new Vector2(CamsPos.x, CamsPos.z));
			}
        }
        else
        {
			if (Vector2.Distance(new Vector2(verts[1].z, verts[1].x), new Vector2(CamsPos.x, CamsPos.z)) < Vector2.Distance(new Vector2(verts[0].z, verts[0].x), new Vector2(CamsPos.x, CamsPos.z)))
            {
				TwoKeyYs.x = verts[1].y;
				TwoKeyYs.y = verts[0].y;
				TwoKeyYs.z = Vector2.Distance(new Vector2(verts[1].z, verts[1].x), new Vector2(CamsPos.x, CamsPos.z)) / Vector2.Distance(new Vector2(verts[0].z, verts[0].x), new Vector2(CamsPos.x, CamsPos.z));
			}
			 
		}
		float CriticalY = Mathf.Lerp(TwoKeyYs.y, TwoKeyYs.x, TwoKeyYs.z);
		//	float CriticalY = (verts[0].y + verts[1].y + verts[2].y) / 3;
		return CriticalY;
    }
	public bool CheckIfObjIsInArr(Vector3[] Arr, Vector3 obj)
    {
		bool check = false;
		if (Arr != null)
		{
			for (int i = 0; i < Arr.Length; i++)
			{
				if (obj == Arr[i]) check = true;
			}
		}
			return check;
    }
	public bool CheckIfObjIsInArr_GO(GameObject[] Arr, GameObject obj)
	{
		bool check = false;
		if (Arr != null)
		{
			for (int i = 0; i < Arr.Length; i++)
			{
				if (obj == Arr[i]) check = true;
			}
		}
		return check;
	}
}
