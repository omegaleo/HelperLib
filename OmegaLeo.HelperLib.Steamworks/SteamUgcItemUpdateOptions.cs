using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(SteamUgcItemUpdateOptions), "Declarative update payload for workshop item editing convenience APIs.", null, null)]
    public class SteamUgcItemUpdateOptions
    {
        [Documentation(nameof(Title), "Workshop item title.", null, null)]
        public string Title { get; set; }

        [Documentation(nameof(Description), "Workshop item description.", null, null)]
        public string Description { get; set; }

        [Documentation(nameof(Metadata), "Workshop item metadata string.", null, null)]
        public string Metadata { get; set; }

        [Documentation(nameof(Visibility), "Numeric ERemoteStoragePublishedFileVisibility value.", null, null)]
        public uint? Visibility { get; set; }

        [Documentation(nameof(Tags), "Workshop tag list.", null, null)]
        public string[] Tags { get; set; }

        [Documentation(nameof(ContentFolder), "Absolute path to workshop content folder.", null, null)]
        public string ContentFolder { get; set; }

        [Documentation(nameof(PreviewFile), "Absolute path to workshop preview image.", null, null)]
        public string PreviewFile { get; set; }
    }
}

