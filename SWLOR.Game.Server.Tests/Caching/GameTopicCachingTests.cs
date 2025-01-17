using System.Collections.Generic;
using NUnit.Framework;
using SWLOR.Game.Server.Caching;
using SWLOR.Game.Server.Data.Entity;
using SWLOR.Game.Server.Event.SWLOR;
using SWLOR.Game.Server.Messaging;

namespace SWLOR.Game.Server.Tests.Caching
{
    public class GameTopicCacheTests
    {
        private GameTopicCache _cache;

        [SetUp]
        public void Setup()
        {
            _cache = new GameTopicCache();
        }

        [TearDown]
        public void TearDown()
        {
            _cache = null;
        }


        [Test]
        public void GetByID_OneItem_ReturnsGameTopic()
        {
            // Arrange
            GameTopic entity = new GameTopic {ID = 1};
            
            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity));

            // Assert
            Assert.AreNotSame(entity, _cache.GetByID(1));
        }

        [Test]
        public void GetByID_TwoItems_ReturnsCorrectObject()
        {
            // Arrange
            GameTopic entity1 = new GameTopic { ID = 1};
            GameTopic entity2 = new GameTopic { ID = 2};

            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity2));

            // Assert
            Assert.AreNotSame(entity1, _cache.GetByID(1));
            Assert.AreNotSame(entity2, _cache.GetByID(2));
        }

        [Test]
        public void GetByID_RemovedItem_ReturnsCorrectObject()
        {
            // Arrange
            GameTopic entity1 = new GameTopic { ID = 1};
            GameTopic entity2 = new GameTopic { ID = 2};

            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity2));
            MessageHub.Instance.Publish(new OnCacheObjectDeleted<GameTopic>(entity1));

            // Assert
            Assert.Throws<KeyNotFoundException>(() => { _cache.GetByID(1); });
            Assert.AreNotSame(entity2, _cache.GetByID(2));
        }

        [Test]
        public void GetByID_NoItems_ThrowsKeyNotFoundException()
        {
            // Arrange
            GameTopic entity1 = new GameTopic { ID = 1};
            GameTopic entity2 = new GameTopic { ID = 2};

            // Act
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectSet<GameTopic>(entity2));
            MessageHub.Instance.Publish(new OnCacheObjectDeleted<GameTopic>(entity1));
            MessageHub.Instance.Publish(new OnCacheObjectDeleted<GameTopic>(entity2));

            // Assert
            Assert.Throws<KeyNotFoundException>(() => { _cache.GetByID(1); });
            Assert.Throws<KeyNotFoundException>(() => { _cache.GetByID(2); });

        }
    }
}
