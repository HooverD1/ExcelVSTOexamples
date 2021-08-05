using NUnit.Framework;
using System;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Daily;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today.AddDays(5);
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(5, midpts.Length);
            Assert.AreEqual(6, endpts.Length);
        }
        [Test]
        public void Test2()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Daily;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today;
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(1, endpts.Length);
        }
        [Test]
        public void Test3()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Weekly;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today;
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(1, endpts.Length);
        }
        [Test]
        public void Test4()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Monthly;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today;
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(1, endpts.Length);
        }
        [Test]
        public void Test5()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Yearly;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today;
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(1, endpts.Length);
        }
        [Test]
        public void Test6()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Yearly;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today.AddDays(1);
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(1, endpts.Length);
        }
        [Test]
        public void Test7()
        {
            double intLength = 1;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Daily;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today.AddDays(1);
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(2, endpts.Length);
        }
        [Test]
        public void Test8()
        {
            double intLength = 2;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Daily;
            DateTime sDate = DateTime.Today;
            DateTime eDate = DateTime.Today.AddDays(1);
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(1, endpts.Length);
        }
        [Test]
        public void Test9()
        {
            double intLength = 2;
            DNA_Test.Scheduler.Scheduler.Interval intType = DNA_Test.Scheduler.Scheduler.Interval.Monthly;
            DateTime sDate = new DateTime(2021, 1, 1);
            DateTime eDate = new DateTime(2021, 1, 1).AddMonths(2);
            DNA_Test.Scheduler.Scheduler scheduler = new DNA_Test.Scheduler.Scheduler(intLength, intType, sDate, eDate);
            DateTime[] midpts = scheduler.GetMidpoints();
            DateTime[] endpts = scheduler.GetEndpoints();
            Assert.AreEqual(endpts[0].ToOADate(), new DateTime(2021,1,1).ToOADate());
            Assert.AreEqual(endpts[endpts.Length-1].ToOADate(), new DateTime(2021, 3, 1).ToOADate());
            Assert.AreEqual(midpts[0].ToOADate(), new DateTime(2021, 2, 1).ToOADate());
            Assert.AreEqual(1, midpts.Length);
            Assert.AreEqual(2, endpts.Length);
        }
    }
}