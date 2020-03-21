using System;
using System.Reflection;

namespace MusicManagementLib.Helpers
{
    public enum MusicLibrary
    {
        Clementine,
        Kodi
    }

    public enum AudioFileType
    {
        [MusicLibraryFileTypeValue(Clementine = 5)]
        [AudioFileTypeExtension(Extension = "mp3")]
        MP3,
        [MusicLibraryFileTypeValue(Clementine = 2)]
        [AudioFileTypeExtension(Extension = "flac")]
        FLAC,
        [MusicLibraryFileTypeValue(Clementine = 8)]
        [AudioFileTypeExtension(Extension = "ogg")]
        OGG_VORBIS,
        [MusicLibraryFileTypeValue(Clementine = 10)]
        [AudioFileTypeExtension(Extension = "wav")]
        WAV,
        [MusicLibraryFileTypeValue(Clementine = 17)]
        [AudioFileTypeExtension(Extension = "ape")]
        APE
    }

    public static class EnumHelper
    {
        public static object GetMusicLibraryFileTypeValue(this AudioFileType type, MusicLibrary library)
        {
            var musicLibraryFileTypeAttribute = typeof(AudioFileType).GetField(type.ToString())?.GetCustomAttribute<MusicLibraryFileTypeValue>();
            if (musicLibraryFileTypeAttribute == null)
                throw new Exception($"AudioFileType enum's field {type.ToString()} is not updated with the proper MusicLibraryFileTypeValue attribute");

            switch (library)
            {
                case MusicLibrary.Clementine:
                    return musicLibraryFileTypeAttribute.Clementine;
                default:
                    return null;
            }
        }

        public static string GetAudioTypeExtension(this AudioFileType type)
        {
            var audioFileTypeExtensionAttribute = typeof(AudioFileType).GetField(type.ToString())?.GetCustomAttribute<AudioFileTypeExtension>();
            if (audioFileTypeExtensionAttribute == null)
                throw new Exception($"AudioFileType enum's field {type.ToString()} is not updated with the proper AudioFileTypeExtension attribute");

            return audioFileTypeExtensionAttribute.Extension;
        }

        public static AudioFileType? GetAudioTypeFromExtension(string extension)
        {
            foreach (var field in typeof(AudioFileType).GetFields())
            {
                var audioFileTypeExtensionAttribute = field.GetCustomAttribute<AudioFileTypeExtension>();
                if (audioFileTypeExtensionAttribute?.Extension == extension.Trim(new char[] { '.', ' ' }).ToLower())
                    return (AudioFileType)field.GetValue(null);
            }

            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class MusicLibraryFileTypeValue : Attribute
    {
        public int Clementine { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AudioFileTypeExtension : Attribute
    {
        public string Extension { get; set; }
    }

    /* Below is Clementines enum definition for each supported filetypes:
     * 
     * enum FileType
     * {
     *    Type_Unknown = 0,
     *    Type_Asf = 1,
     *    Type_Flac = 2,
     *    Type_Mp4 = 3,
     *    Type_Mpc = 4,
     *    Type_Mpeg = 5,
     *    Type_OggFlac = 6,
     *    Type_OggSpeex = 7,
     *    Type_OggVorbis = 8,
     *    Type_Aiff = 9,
     *    Type_Wav = 10,
     *    Type_TrueAudio = 11,
     *    Type_Cdda = 12,
     *    Type_OggOpus = 13,
     *    Type_WavPack = 14,
     *    Type_Spc = 15,
     *    Type_VGM = 16,
     *    Type_APE = 17,
     *    Type_Stream = 99,
     * };
     * 
     */
}
