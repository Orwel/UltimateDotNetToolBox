using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolBox;

namespace ToolBoxTest
{
    [TestClass]
    public class EnumHelperTest
    {
        enum TestEnum
        {
            Value1,
            Value2,
            Value3,
            Value4,
            Value5
        }

        [TestMethod]
        public void Parse_Test()
        {
            //Normal case
            {
                TestEnum r = EnumHelper.Parse<TestEnum>("Value1", TestEnum.Value5);
                Assert.AreEqual(TestEnum.Value1, r);
            }
            //Bad value case
            {
                TestEnum r = EnumHelper.Parse<TestEnum>("Val1", TestEnum.Value5);
                Assert.AreEqual(TestEnum.Value5, r);
            }
            //Null value case
            {
                TestEnum r = EnumHelper.Parse<TestEnum>(null, TestEnum.Value5);
                Assert.AreEqual(TestEnum.Value5, r);
            }
            //Not enum value case
            {
                try
                {
                    int arg = EnumHelper.Parse<int>("69", 55);
                    Assert.Fail("No Argument Exception");
                }
                catch(ArgumentException)
                { }
            }
        }

        [TestMethod]
        public void TryParse_Test()
        {
            
            //Normal case
            {
                TestEnum r = TestEnum.Value5;
                Assert.AreEqual(true, EnumHelper.TryParse<TestEnum>("Value1", ref r));
                Assert.AreEqual(TestEnum.Value1, r);
            }
            //Bad value case
            {
                TestEnum r = TestEnum.Value5;
                Assert.AreEqual(false, EnumHelper.TryParse<TestEnum>("Val1", ref r));
                Assert.AreEqual(TestEnum.Value5, r);
            }
            //Null value case
            {
                TestEnum r = TestEnum.Value5;
                Assert.AreEqual(false, EnumHelper.TryParse<TestEnum>(null, ref r));
                Assert.AreEqual(TestEnum.Value5, r);
            }
            //Not enum value case
            {
                try
                {
                    int arg = 55;
                    EnumHelper.TryParse<int>("69", ref arg);
                    Assert.Fail("No Argument Exception");
                }
                catch (ArgumentException)
                {  }
            }
        }

        [TestMethod]
        public void ToIntArray_Test()
        {
            TestEnum[] enumArray = { TestEnum.Value1, TestEnum.Value2, TestEnum.Value3,
                                     TestEnum.Value4, TestEnum.Value5 };
            int[] intArray = EnumHelper.ToIntArray(enumArray);
            for (int i = 0; i < 5; i++ )
                Assert.AreEqual(i, intArray[i]);
        }
    
        [TestMethod]
        public void FromIntArray_Test()
        {
            //Normal case
            int[] arrayInt = { 0, 1, 2, 3, 4, 5 };
            TestEnum[] enumArray = EnumHelper.FromIntArray<TestEnum>(arrayInt);
            Assert.AreEqual(TestEnum.Value1, enumArray[0]);
            Assert.AreEqual(TestEnum.Value2, enumArray[1]);
            Assert.AreEqual(TestEnum.Value3, enumArray[2]);
            Assert.AreEqual(TestEnum.Value4, enumArray[3]);
            Assert.AreEqual(TestEnum.Value5, enumArray[4]);
            Assert.AreEqual(5, (int)enumArray[5]);

            //Type is not enum
            try
            {
                long[] arrayLong = EnumHelper.FromIntArray<long>(arrayInt);
                Assert.Fail("No ArgumentException");
            }
            catch (ArgumentException)
            { }
        }
    }
}
