using System.Collections.Generic;

namespace EGamePlay.Combat
{
    public class IntNumericItem
    {
        public int Value;
        public int Base { get; set; }
        public int Add { get; set; }
        public int PctAdd { get; set; }
        public int FinalAdd { get; set; }
        public int FinalPctAdd { get; set; }

        public int FinalValue()
        {
            return Value;
        }

        public void Update()
        {
            // 一个数值可能会多种情况影响 比如速度 加个buff可能增加速度绝对值100 也有些buff增加10%速度 所以一个值可以由5个值进行控制其最终结果
            Value = (int)((((Base + Add) * (100 + PctAdd) / 100f) + FinalAdd) * (100 + FinalPctAdd) / 100f);
        }
    }

    public class FloatNumericItem
    {
        public float Value;
        public float Base { get; set; }
        public float Add { get; set; }
        public int   PctAdd { get; set; }
        public float FinalAdd { get; set; }
        public int   FinalPctAdd { get; set; }

        public float FinalValue()
        {
            return Value;
        }

        public void Update()
        {
            // 一个数值可能会多种情况影响 比如速度 加个buff可能增加速度绝对值100 也有些buff增加10%速度 所以一个值可以由5个值进行控制其最终结果
            Value = (((Base + Add) * (100 + PctAdd) / 100f) + FinalAdd) * (100 + FinalPctAdd) / 100f;
        }
    }

    public class CombatNumericBox
	{
		//public Dictionary<int, NumericItem> NumericItems = new Dictionary<int, NumericItem>();
		//public Dictionary<int, int> IntNumericBox = new Dictionary<int, int>();
        //public Dictionary<int, float> FloatNumericBox = new Dictionary<int, float>();
        public IntNumericItem PhysicAttack_I = new IntNumericItem();
        public FloatNumericItem CriticalProb_F = new FloatNumericItem();


        public void Initialize()
		{
            // 这里初始化base值
            PhysicAttack_I.Base = 1;
            CriticalProb_F.Base = 0.5f;
        }

        //public int Get(IntNumericType numericType)
        //{
        //    return IntNumericBox[(int)numericType];
        //}

        //public int GetBase(IntNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 1;
        //    return IntNumericBox[bas];
        //}

        //public int GetAdd(IntNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 2;
        //    return IntNumericBox[bas];
        //}

        //public float GetPct(IntNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 3;
        //    return IntNumericBox[bas] / 10000f;
        //}

        //public int GetFinalAdd(IntNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 4;
        //    return IntNumericBox[bas];
        //}

        //public int GetFinalPct(IntNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 5;
        //    return IntNumericBox[bas];
        //}

        //#region Float
        //public float Get(FloatNumericType numericType)
        //{
        //    return FloatNumericBox[(int)numericType];
        //}

        //public float GetBase(FloatNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 1;
        //    return FloatNumericBox[bas];
        //}

        //public float GetAdd(FloatNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 2;
        //    return FloatNumericBox[bas];
        //}

        //public float GetPct(FloatNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 3;
        //    return FloatNumericBox[bas];
        //}

        //public float GetFinalAdd(FloatNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 4;
        //    return FloatNumericBox[bas];
        //}

        //public float GetFinalPct(FloatNumericType numericType)
        //{
        //    int bas = (int)numericType * 10 + 5;
        //    return FloatNumericBox[bas];
        //}
        //#endregion

        //public void Update(IntNumericType numericType)
		//{
		//	//int final = (int)numericType / 10;
		//	//int bas = final * 10 + 1; 
		//	//int add = final * 10 + 2;
		//	//int pct = final * 10 + 3;
		//	//int finalAdd = final * 10 + 4;
		//	//int finalPct = final * 10 + 5;

		//	// 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
		//	// final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
		//	int result = (int)(((this.GetBase(numericType) + this.GetAdd(numericType)) * (100 + this.GetPct(pct)) / 100f + this.GetByKey(finalAdd)) * (100 + this.GetAsFloat(finalPct)) / 100f * 10000);
		//	this.IntNumericBox[(int)numericType] = result;
		//}

		//public void Update(FloatNumericType numericType)
		//{
		//	//int final = (int)numericType / 10;
		//	//int bas = final * 10 + 1;
		//	//int add = final * 10 + 2;
		//	//int pct = final * 10 + 3;
		//	//int finalAdd = final * 10 + 4;
		//	//int finalPct = final * 10 + 5;

		//	// 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
		//	// final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
		//	int result = (int)(((this.GetByKey(bas) + this.GetByKey(add)) * (100 + this.GetAsFloat(pct)) / 100f + this.GetByKey(finalAdd)) * (100 + this.GetAsFloat(finalPct)) / 100f * 10000);
		//	this.FloatNumericBox[(int)numericType] = result;
		//}
	}
}