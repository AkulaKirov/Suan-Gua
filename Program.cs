using ConsoleApp1;

class Program
{
    public static int Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        uint 先, 后;

        var _64 = new _64Gua();

        Console.WriteLine("请输入两个正整数: ");
        var read = Console.ReadLine().Trim().Split().ToList();
        read.RemoveAll(x => x == "");
        先 = uint.Parse(read[0]);
        后 = uint.Parse(read[1]);

        //先 = (uint)new Random().Next();
        //后 = (uint)new Random().Next();

        卦象 本卦 = 卦象.起卦(先, 后);
        Console.WriteLine($"体: {本卦.上卦} 用: {本卦.下卦} 动爻: {本卦.动爻}");
        Console.WriteLine($"本卦: {本卦} 上{(卦象.八卦)本卦.上卦}下{(卦象.八卦)本卦.下卦} {本卦.解卦()}");

        卦象 互卦 = 本卦.互卦();
        Console.WriteLine($"互卦: {互卦} 上{(卦象.八卦)互卦.上卦}下{(卦象.八卦)互卦.下卦} {互卦.解卦()}");

        卦象 变卦 = 本卦.变卦();
        Console.WriteLine($"变卦: {变卦} 上{(卦象.八卦)变卦.上卦}下{(卦象.八卦)变卦.下卦} {变卦.解卦()}");
        Console.WriteLine($"卦解(以求事为例):");
        Console.WriteLine($"开端: {分类占断.占断(本卦, 分类占断.求谋)}");
        Console.WriteLine($"发展: {分类占断.占断(互卦, 分类占断.求谋)}");
        Console.WriteLine($"结局: {分类占断.占断(变卦, 分类占断.求谋)}");
        var (name, desc) = _64.Get(((卦象.八卦)本卦.上卦).ToString() + ((卦象.八卦)本卦.下卦).ToString());
        Console.WriteLine($"\n{name}\n{desc}\n");
        (name, desc) = _64.Get(((卦象.八卦)互卦.上卦).ToString() + ((卦象.八卦)互卦.下卦).ToString());
        Console.WriteLine($"\n{name}\n{desc}\n");
        (name, desc) = _64.Get(((卦象.八卦)变卦.上卦).ToString() + ((卦象.八卦)变卦.下卦).ToString());
        Console.WriteLine($"\n{name}\n{desc}\n");
        return 0;
    }

    class 分类占断
    {
        public static List<string> 求谋
        {
            get
            {
                return new List<string>
                {
                    "求事可成，但需时间",
                    "求事不成，事反有害",
                    "所求之事，不谋而成",
                    "所求之事，事倍功半",
                    "称心如意，势在必得"
                };
            }
        }

        public static string 占断(卦象 卦, List<string> 分类)
        {
            return 分类.ElementAt(((int)卦.解卦()));
        }
    }

    class 卦象
    {
        public byte 上卦;
        public byte 下卦;
        public byte 动爻;

        public enum 卦解
        {
            体生用,
            用生体,
            体克用,
            用克体,
            体用比和
        };

        public enum 五行
        {
            金,
            木,
            水,
            火,
            土
        };

        public enum 八卦
        {
            坤,
            震,
            坎,
            兑,
            艮,
            离,
            巽,
            乾,
        };

        private 卦象() { }

        private 卦象(byte 卦) { this.上卦 = (byte)(卦 >> 3); this.下卦 = (byte)(卦 & 0b000111); }

        public static 卦象 起卦(uint 先, uint 后)
        {
            卦象 卦 = new();
            卦.上卦 = (byte)(先 % 8);
            卦.下卦 = (byte)(后 % 8);
            卦.动爻 = (byte)(Math.Abs(先 + 后) % 6);
            return 卦;
        }

        public 卦象 互卦()
        {
            byte 下互 = (byte)((0b1110 & this) >> 1);
            byte 上互 = (byte)((0b11100 & this) >> 2);
            return new 卦象() { 上卦 = 上互, 下卦 = 下互 };
        }

        public 卦象 变卦()
        {
            var mask = 1 << 动爻;
            return new((byte)((this & (byte)(~mask)) | (this ^ (byte)mask)));
        }

        public 卦解 解卦()
        {
            var 先 = 获取五行(上卦);
            var 后 = 获取五行(下卦);
            if (相生(先, 后))
            {
                return 卦解.体生用;
            }
            else if (相生(后, 先))
            {
                return 卦解.用生体;
            }
            else if (相克(先, 后))
            {
                return 卦解.体克用;
            }
            else if (相克(后, 先))
            {
                return 卦解.用克体;
            }
            else
            {
                return 卦解.体用比和;
            }

        }

        public static bool 相生(五行 先, 五行 后)
        {
            return
                (先 == 五行.金 && 后 == 五行.水) ||
                (先 == 五行.水 && 后 == 五行.木) ||
                (先 == 五行.木 && 后 == 五行.火) ||
                (先 == 五行.火 && 后 == 五行.土) ||
                (先 == 五行.土 && 后 == 五行.金);
        }

        public static bool 相克(五行 先, 五行 后)
        {
            return
                (先 == 五行.金 && 后 == 五行.木) ||
                (先 == 五行.木 && 后 == 五行.土) ||
                (先 == 五行.土 && 后 == 五行.水) ||
                (先 == 五行.水 && 后 == 五行.火) ||
                (先 == 五行.火 && 后 == 五行.金);
        }

        public static 五行 获取五行(byte 卦)
        {
            switch ((八卦)卦)
            {
                case 八卦.乾:
                case 八卦.兑: return 五行.金;

                case 八卦.震:
                case 八卦.巽: return 五行.木;

                case 八卦.坎: return 五行.水;

                case 八卦.离: return 五行.火;

                case 八卦.艮:
                case 八卦.坤: return 五行.土;

                default: throw new Exception();
            }
        }

        public override string ToString()
        {
            return $"{Convert.ToString((上卦 << 3) + 下卦, 2).PadLeft(6, '0')}";
        }

        public static byte operator &(byte a, 卦象 b)
        {
            return (byte)(a & ((b.上卦 << 3) + b.下卦));
        }
        public static byte operator &(卦象 b, byte a)
        {
            return a & b;
        }
        public static byte operator ^(byte a, 卦象 b)
        {
            return (byte)(a ^ ((b.上卦 << 3) + b.下卦));
        }
        public static byte operator ^(卦象 b, byte a)
        {
            return (byte)((b.上卦 << 3) + b.下卦 ^ a);
        }
    }
}