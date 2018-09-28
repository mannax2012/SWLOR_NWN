﻿using System.Linq;
using NWN;
using SWLOR.Game.Server.Data.Contracts;
using SWLOR.Game.Server.Data.Entities;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.GameObject;
using SWLOR.Game.Server.Item.Contracts;
using SWLOR.Game.Server.Service.Contracts;
using SWLOR.Game.Server.ValueObject;
using static NWN.NWScript;

namespace SWLOR.Game.Server.Item.Medicine
{
    public class ResuscitationKit: IActionItem
    {

        private readonly INWScript _;
        private readonly IDataContext _db;
        private readonly ISkillService _skill;
        private readonly IRandomService _random;
        private readonly IPerkService _perk;
        private readonly IPlayerStatService _playerStat;

        public ResuscitationKit(
            INWScript script,
            IDataContext db,
            ISkillService skill,
            IRandomService random,
            IPerkService perk,
            IPlayerStatService playerStat)
        {
            _ = script;
            _db = db;
            _skill = skill;
            _random = random;
            _perk = perk;
            _playerStat = playerStat;
        }
        public CustomData StartUseItem(NWCreature user, NWItem item, NWObject target, Location targetLocation)
        {
            user.SendMessage("You begin resuscitating " + target.Name + "...");
            return null;
        }

        public void ApplyEffects(NWCreature user, NWItem item, NWObject target, Location targetLocation, CustomData customData)
        {
            NWPlayer player = user.Object;
            PCSkill skill = _skill.GetPCSkill(player, SkillType.Medicine);
            int perkLevel = _perk.GetPCPerkLevel(player, PerkType.ResuscitationDevices);
            int rank = item.GetLocalInt("RANK");
            int baseHeal;

            switch (rank)
            {
                case 1:
                    baseHeal = 1;
                    break;
                case 2:
                    baseHeal = 11;
                    break;
                case 3:
                    baseHeal = 31;
                    break;
                case 4:
                    baseHeal = 51;
                    break;
                default: return;
            }

            baseHeal += perkLevel * 2;
            baseHeal += _playerStat.EffectiveMedicineBonus(player) / 2;

            PlayerCharacter dbPlayer = _db.PlayerCharacters.Single(x => x.PlayerID == user.GlobalID);
            int hpRecover = (int)(target.MaxHP * (0.01f * baseHeal));
            int fpRecover = (int) (dbPlayer.MaxFP * (0.01f * baseHeal));

            _.ApplyEffectToObject(DURATION_TYPE_INSTANT, _.EffectResurrection(), target);
            _.ApplyEffectToObject(DURATION_TYPE_INSTANT, _.EffectHeal(hpRecover), target);
            dbPlayer.CurrentFP = fpRecover;

            _db.SaveChanges();
            player.SendMessage("You successfully resuscitate " + target.Name + "!");
            int xp = (int)_skill.CalculateRegisteredSkillLevelAdjustedXP(600, item.RecommendedLevel, skill.Rank);
            _skill.GiveSkillXP(player, SkillType.Medicine, xp);
        }

        public float Seconds(NWCreature user, NWItem item, NWObject target, Location targetLocation, CustomData customData)
        {
            if (_random.Random(100) + 1 <= _perk.GetPCPerkLevel((NWPlayer)user, PerkType.SpeedyFirstAid) * 10)
            {
                return 0.1f;
            }

            PCSkill skill = _skill.GetPCSkill((NWPlayer)user, SkillType.Medicine);
            return 12.0f - (skill.Rank * 0.1f);
        }

        public bool FaceTarget()
        {
            return false;
        }

        public int AnimationID()
        {
            return ANIMATION_LOOPING_GET_LOW;
        }

        public float MaxDistance(NWCreature user, NWItem item, NWObject target, Location targetLocation)
        {
            return 3.5f + _perk.GetPCPerkLevel(user.Object, PerkType.RangedHealing);
        }

        public bool ReducesItemCharge(NWCreature user, NWItem item, NWObject target, Location targetLocation, CustomData customData)
        {
            int consumeChance = _perk.GetPCPerkLevel((NWPlayer)user, PerkType.FrugalMedic) * 10;
            return _random.Random(100) + 1 > consumeChance;
        }

        public string IsValidTarget(NWCreature user, NWItem item, NWObject target, Location targetLocation)
        {
            if (_.GetIsPC(target.Object) == FALSE || _.GetIsDM(target.Object) == TRUE)
            {
                return "Only players may be targeted with this item.";
            }

            if (target.CurrentHP <= -11)
            {
                return "Your target is not dead.";
            }

            if (user.IsInCombat)
            {
                return "You are in combat.";
            }

            int perkLevel = _perk.GetPCPerkLevel(user.Object, PerkType.ResuscitationDevices);
            int requiredLevel = item.GetLocalInt("RANK");

            if (perkLevel < requiredLevel)
            {
                return "You must have the Resuscitation Devices perk at level " + requiredLevel + " to use this item.";
            }

            return null;
        }

        public bool AllowLocationTarget()
        {
            return false;
        }
    }
}