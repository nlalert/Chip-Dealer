
using System;
using System.Collections.Generic;
public class Relics
{
      public enum RelicType
      {
            PotatoChip,
            ProcessingChip,
            ChippedChip
      }

      public static RelicType GetRandomRelic()
      {
           return (RelicType)Singleton.Instance.Random.Next(0,Enum.GetValues(typeof(RelicType)).Length);
      }

      public static int GetRelicPrice(RelicType relicType)
      {

            Dictionary<RelicType, int> _price = new Dictionary<RelicType, int>();

            _price.Add(RelicType.PotatoChip, 20);
            _price.Add(RelicType.ProcessingChip, 15);
            _price.Add(RelicType.ChippedChip, 50);

            return _price[relicType];
      }

      public static int GetRelicRarity(RelicType relicType)
      {

            Dictionary<RelicType, int> _rarity = new Dictionary<RelicType, int>();

            _rarity.Add(RelicType.PotatoChip, 0);
            _rarity.Add(RelicType.ProcessingChip, 1);
            _rarity.Add(RelicType.ChippedChip, 2);

            return _rarity[relicType];
      }

      public static String GetRelicDescriptions(RelicType relicType)
      {

            Dictionary<RelicType, String> _descriptions = new Dictionary<RelicType, String>();

            _descriptions.Add(RelicType.PotatoChip, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ProcessingChip, "Does nothing.\nFor now.");
            _descriptions.Add(RelicType.ChippedChip, "Does nothing.\nFor now.");

            return _descriptions[relicType];
      }

}

