﻿using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace TinyTweaks;

[StaticConstructorOnStartup]
public static class NonPublicProperties
{
    public static Func<Building_TurretGun, bool> Building_TurretGun_get_PlayerControlled =
        (Func<Building_TurretGun, bool>)
        Delegate.CreateDelegate(typeof(Func<Building_TurretGun, bool>), null,
            typeof(Building_TurretGun)
                .GetProperty("PlayerControlled", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetGetMethod(true)!);

    public static Action<TurretTop, float> TurretTop_set_CurRotation = (Action<TurretTop, float>)
        Delegate.CreateDelegate(typeof(Action<TurretTop, float>), null,
            AccessTools.Property(typeof(TurretTop), "CurRotation").GetSetMethod(true));

    public static readonly Func<WITab, Caravan> WITab_get_SelCaravan = (Func<WITab, Caravan>)
        Delegate.CreateDelegate(typeof(Func<WITab, Caravan>), null,
            typeof(WITab).GetProperty("SelCaravan", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetGetMethod(true)!);
}