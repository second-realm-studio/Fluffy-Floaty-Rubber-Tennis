using PlayerEntities;
using UnityEngine;
using XiheFramework.Combat.Action;
using XiheFramework.Runtime;

namespace Actions {
    public abstract class TennisPlayerActionEntityBase : ActionEntity {
        [HideInInspector]
        public TennisPlayerEntity owner;

        public override string EntityGroupName => "TennisPlayerActionEntity";

        protected override void OnActionInit() {
            if (!Game.Entity.IsEntityExisted(OwnerId)) {
                Game.Entity.DestroyEntity(EntityId);
                return;
            }

            owner = Game.Entity.GetEntity<TennisPlayerEntity>(OwnerId);
        }

        public override void OnUpdateCallback() {
            if (owner == null) {
                Game.Entity.DestroyEntity(EntityId);
            }

            base.OnUpdateCallback();
        }
    }
}