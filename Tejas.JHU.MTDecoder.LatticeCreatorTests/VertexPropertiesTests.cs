using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;
using QuickGraph;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.LatticeCreatorTests
{
    [TestFixture]
    class VertexPropertiesTests
    {
        [Test]
        public void ListEquality()
        {
            IList <int> x = new List<int>();
            IList<int> y = new List<int>();
            x.Add(1);
            x.Add(2);
            y.Add(1);
            y.Add(2);
            var res = x.Except(y).ToList();
            Assert.True(res.Count==0);
            x.Add(3);
            res = x.Except(y).ToList();
            Assert.True(res.Count>0);
            x.RemoveAt(2);
            x.Insert(2,4);
            Assert.True(x.Count==3);

            OrderedSet<int> a = new OrderedSet<int>();
            OrderedSet<int> z = new OrderedSet<int>();
            a.Add(0);
            a.Add(1);
            a.Add(2);
            var s = a.GetFirst();
            Assert.True(s==0);
            z.Add(0);
            z.Add(1);

            List<int> r = a.Except(z).ToList();
            Assert.That(r.Count>0);

            Assert.True(a.Contains(2));
        }


        [Test]
        public void VertexPropertiesEqualityTest()
        {
            OrderedSet<int> x = new OrderedSet<int>();
            x.Add(0);
            x.Add(1);
            VertexProperties v1  = new VertexProperties(0.0,"<s> <s>",x);
            OrderedSet<int> y = new OrderedSet<int>();
            y.Add(0);
            y.Add(1);
            y.Add(2);

            VertexProperties v2 = new VertexProperties(0.4, "<s> <>", y);
           
            Assert.False(v1.Equals(v2));
            v2.CoverageVector.Remove(2);
            v2.State = "<s> <s>";
            Assert.True(v1.Equals(v2));

        }

        [Test]        
        public void CompareToTest()
        {
            OrderedSet<int> x = new OrderedSet<int>();
            OrderedSet<int> y = new OrderedSet<int>();
            x.Add(0);
            y.Add(0);
            OrderedSet<VertexProperties> set = new OrderedSet<VertexProperties>();
            VertexProperties v1 = new VertexProperties(0.0, "<s> <s>", x);
            VertexProperties v2 = new VertexProperties(0.0, "<s> <>", y);
            set.Add(v1);
            set.Add(v2);
            Assert.True(set.Count==2);
            VertexProperties r;
            set.TryGetItem(v1, out r);
            Assert.True(r.Equals(v1));
            Assert.True(set.Contains(v1));
            Assert.True(set.Contains(new VertexProperties(0.22,"<s> <s>",x)));
        }

        [Test]
        public void GraphAddTest()
        {
            BidirectionalGraph<VertexProperties,EdgeProperties> g = new BidirectionalGraph<VertexProperties, EdgeProperties>();

            OrderedSet<int> x = new OrderedSet<int>();
            OrderedSet<int> y = new OrderedSet<int>();
            x.Add(0);
            y.Add(0);
            VertexProperties v1 = new VertexProperties(0.0, "<s> <s>", x);
            VertexProperties v2 = new VertexProperties(0.0, "<s> <>", y);
            g.AddVertex(v1);
            //g.AddVertex(v2);
            OrderedSet<VertexProperties> v = new OrderedSet<VertexProperties>();
            v.Add(v1);
            var q = v.GetFirst();
            q.ForwardCost = .09999;
            Assert.True(g.ContainsVertex(q) && g.Vertices.FirstOrDefault().ForwardCost==q.ForwardCost);
            
        }







    }
}
