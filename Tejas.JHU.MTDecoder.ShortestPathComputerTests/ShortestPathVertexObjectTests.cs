using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tejas.JHU.MTDecoder.LatticeCreator;
using Tejas.JHU.MTDecoder.ShortestPathComputer;
using Wintellect.PowerCollections;

namespace Tejas.JHU.MTDecoder.ShortestPathComputerTests
{
    [TestFixture]
    class ShortestPathVertexObjectTests
    {

        [Test]
        public void ContainsTest()
        {
            OrderedSet<ShortestPathVertexObject> x = new OrderedSet<ShortestPathVertexObject>();
            ShortestPathVertexObject v  = new ShortestPathVertexObject(new VertexProperties(0.0,"<s> <s>",new OrderedSet<int>()),0.0);
            x.Add(v);
            ShortestPathVertexObject v1 = new ShortestPathVertexObject(new VertexProperties(0.4, "<s> <s>", new OrderedSet<int>()), 0.2);
            Assert.True(x.Contains(v1));

        }

        [Test]
        public void ReferenceChangeTest()
        {
            
                OrderedSet<ShortestPathVertexObject> x = new OrderedSet<ShortestPathVertexObject>();
                ShortestPathVertexObject v = new ShortestPathVertexObject(new VertexProperties(0.0, "<s> <s>", new OrderedSet<int>()), 0.0);
                ShortestPathVertexObject v2 = new ShortestPathVertexObject(new VertexProperties(0.3, "<s> <s>", new OrderedSet<int>()), 0.4);
                x.Add(v);
                ShortestPathVertexObject v1;
                x.TryGetItem(v2, out v1);
                Assert.True(v1.Distance==v.Distance);
                v1.Distance = 3.3;
                ShortestPathVertexObject a;
                x.TryGetItem(v2, out a);
                Assert.True(a.Distance==3.3);

        }
    }
}
