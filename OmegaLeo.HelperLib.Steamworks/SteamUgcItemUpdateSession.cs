using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(SteamUgcItemUpdateSession), "Fluent helper for editing and submitting a workshop item update.", null, null)]
    public class SteamUgcItemUpdateSession
    {
        private readonly SteamUgcManager _ugcManager;
        private readonly uint _consumerAppId;
        private readonly ulong _publishedFileId;
        private readonly SteamUgcItemUpdateOptions _options = new SteamUgcItemUpdateOptions();

        internal SteamUgcItemUpdateSession(SteamUgcManager ugcManager, uint consumerAppId, ulong publishedFileId)
        {
            _ugcManager = ugcManager;
            _consumerAppId = consumerAppId;
            _publishedFileId = publishedFileId;
        }

        public SteamUgcItemUpdateSession WithTitle(string title)
        {
            _options.Title = title;
            return this;
        }

        public SteamUgcItemUpdateSession WithDescription(string description)
        {
            _options.Description = description;
            return this;
        }

        public SteamUgcItemUpdateSession WithMetadata(string metadata)
        {
            _options.Metadata = metadata;
            return this;
        }

        public SteamUgcItemUpdateSession WithVisibility(uint visibility)
        {
            _options.Visibility = visibility;
            return this;
        }

        public SteamUgcItemUpdateSession WithTags(params string[] tags)
        {
            _options.Tags = tags;
            return this;
        }

        public SteamUgcItemUpdateSession WithContentFolder(string contentFolder)
        {
            _options.ContentFolder = contentFolder;
            return this;
        }

        public SteamUgcItemUpdateSession WithPreviewFile(string previewFile)
        {
            _options.PreviewFile = previewFile;
            return this;
        }

        [Documentation(nameof(Submit), "Submits the fluent update session and returns a Steam API call handle.", new[]
        {
            "changeNote: Changelog note for this update.",
            "failedStep: Name of the failed step if submission returns 0."
        }, null)]
        public ulong Submit(string changeNote, out string failedStep)
        {
            return _ugcManager.UpdateItem(_consumerAppId, _publishedFileId, _options, changeNote, out failedStep);
        }
    }
}

