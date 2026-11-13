using UnityEditor;
using UnityEngine;

public static partial class PrototypeSetup
{
    const string AudioPath = "Assets/Audio/{0}.wav";

    static void BuildAudio(Transform root)
    {
        var audio = Child<GameAudio>("Audio", root);
        AudioSource sfx = audio.gameObject.AddComponent<AudioSource>();
        AudioSource luckVoice = audio.gameObject.AddComponent<AudioSource>();
        AudioSource music = audio.gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = luckVoice.playOnAwake = music.playOnAwake = false;
        music.loop = true;
        music.volume = 0.5f;

        var run = Object.FindAnyObjectByType<CommuteRun>();
        Wire(audio, ("config", AssetDatabase.LoadAssetAtPath<GameConfig>(ConfigPath)), ("run", run),
            ("combo", run.GetComponent<ComboMeter>()), ("sfx", sfx), ("luckVoice", luckVoice), ("music", music),
            ("gain", Clip("Gain")), ("loss", Clip("Loss")), ("comboUp", Clip("ComboUp")), ("fever", Clip("Fever")),
            ("homeMusic", Clip("BgmHome", true)), ("commuteMusic", Clip("BgmCommute", true)));

        Wire(Object.FindAnyObjectByType<SlotGame>(FindObjectsInactive.Include),
            ("sfx", sfx), ("spinTick", Clip("SlotTick")), ("reelStop", Clip("ReelStop")), ("jackpot", Clip("Jackpot")));
        Wire(Object.FindAnyObjectByType<PickupFx>(), ("sfx", sfx), ("pop", Clip("Pop")));
        Wire(Object.FindAnyObjectByType<Pedestrian>(), ("sfx", sfx), ("bumpSound", Clip("Bump")));
        Wire(Object.FindAnyObjectByType<GameFlow>(), ("sfx", sfx), ("click", Clip("Click")), ("doorSound", Clip("Door")));
        Wire(Object.FindAnyObjectByType<Newspaper>(FindObjectsInactive.Include), ("sfx", sfx), ("click", Clip("Click")));
        Wire(Object.FindAnyObjectByType<ResultScreen>(FindObjectsInactive.Include), ("sfx", sfx), ("fanfare", Clip("Jackpot")));
        Object.FindAnyObjectByType<RainDay>().GetComponent<AudioSource>().clip = Clip("Rain", true);
        BuildOptions(music);
    }

    static AudioClip Clip(string name, bool isMusic = false)
    {
        string path = string.Format(AudioPath, name);
        AssetDatabase.ImportAsset(path);
        var importer = (AudioImporter)AssetImporter.GetAtPath(path);
        importer.forceToMono = true;
        AudioImporterSampleSettings settings = importer.defaultSampleSettings;
        settings.loadType = isMusic ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
        settings.compressionFormat = AudioCompressionFormat.Vorbis;
        settings.quality = isMusic ? 0.5f : 0.7f;
        importer.defaultSampleSettings = settings;
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }
}
