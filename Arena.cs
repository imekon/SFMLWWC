﻿using TinyMessenger;

namespace WWC
{
    internal static class Arena
    {
        public static void Battle(TinyMessengerHub hub, Actor actor1, Actor actor2)
        {
            var random = new Random();

            // actor1 against actor2
            Attack(hub, random, actor1, actor2, 4);

            // actor2 response
            if (random.Next(100) < 90)
                Attack(hub, random, actor2, actor1, 4);
        }

        private static void Attack(TinyMessengerHub hub, Random random, Actor actor1, Actor actor2, int damageLimit)
        {
            var attack = random.Next(36);
            var damage = random.Next(damageLimit);
            var fumble = (attack <= 4);
            var critical = (attack >= 34);
            var hit = attack < actor1.Strength;
            if (hit)
            {
                actor2.Energy -= critical ? damage * 10 : damage * 5;
                hub.Publish(new StatusMessage(actor1, "(Actor1) hits (Actor2)"));
            }
            if (fumble)
                actor1.Energy -= 5;
        }
    }
}
