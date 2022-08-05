﻿using NUnit.Framework;

namespace AniListNet.Tests;

public class GetMediaTests
{

    [Test]
    public async Task GetCharacterMediaTest()
    {
        var data = await TestObjects.AniClient.GetCharacterMediaAsync(1);
        Console.WriteLine(ObjectDumper.Dump(data));
        Assert.Pass();
    }

    [Test]
    public async Task GetStaffProductionMediaTest()
    {
        var data = await TestObjects.AniClient.GetStaffProductionMediaAsync(95269);
        Console.WriteLine(ObjectDumper.Dump(data));
        Assert.Pass();
    }

    [Test]
    public async Task GetStaffVoicedMediaTest()
    {
        var data = await TestObjects.AniClient.GetStaffVoicedMediaAsync(95269);
        Console.WriteLine(ObjectDumper.Dump(data));
        Assert.Pass();
    }

    [Test]
    public async Task GetStaffVoicedCharactersTest()
    {
        var data = await TestObjects.AniClient.GetStaffVoicedCharactersAsync(95269);
        Console.WriteLine(ObjectDumper.Dump(data));
        Assert.Pass();
    }

    [Test]
    public async Task GetStudioMediaTest()
    {
        var data = await TestObjects.AniClient.GetStudioMediaAsync(1);
        Console.WriteLine(ObjectDumper.Dump(data));
        Assert.Pass();
    }

}