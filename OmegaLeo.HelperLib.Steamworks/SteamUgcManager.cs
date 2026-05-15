using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(SteamUgcManager), "Provides helper methods for common Steam Workshop (UGC) operations such as queries and subscriptions.", null,
        @"```csharp
var ugc = new SteamUgcManager();
var items = ugc.GetSubscribedItems();
```")]
    public class SteamUgcManager
    {
        [Documentation(nameof(BeginUpdateSession), "Begins a fluent UGC update session for an existing workshop item.", new[]
        {
            "consumerAppId: Your game AppId.",
            "publishedFileId: Existing workshop item ID."
        }, null)]
        public SteamUgcItemUpdateSession BeginUpdateSession(uint consumerAppId, ulong publishedFileId)
        {
            return new SteamUgcItemUpdateSession(this, consumerAppId, publishedFileId);
        }

        [Documentation(nameof(ApplyItemUpdate), "Applies all non-null/non-empty fields from a UGC update options object to an existing update handle.", new[]
        {
            "updateHandle: Handle from StartItemUpdate.",
            "options: Options object containing fields to apply.",
            "failedStep: Name of the field that failed to apply, if any."
        }, null)]
        public bool ApplyItemUpdate(ulong updateHandle, SteamUgcItemUpdateOptions options, out string failedStep)
        {
            failedStep = string.Empty;

            if (updateHandle == 0 || options == null)
            {
                failedStep = "InvalidArguments";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(options.Title) && !SetItemTitle(updateHandle, options.Title))
            {
                failedStep = nameof(options.Title);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(options.Description) && !SetItemDescription(updateHandle, options.Description))
            {
                failedStep = nameof(options.Description);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(options.Metadata) && !SetItemMetadata(updateHandle, options.Metadata))
            {
                failedStep = nameof(options.Metadata);
                return false;
            }

            if (options.Visibility.HasValue && !SetItemVisibility(updateHandle, options.Visibility.Value))
            {
                failedStep = nameof(options.Visibility);
                return false;
            }

            if (options.Tags != null && options.Tags.Length > 0 && !SetItemTags(updateHandle, options.Tags))
            {
                failedStep = nameof(options.Tags);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(options.ContentFolder) && !SetItemContent(updateHandle, options.ContentFolder))
            {
                failedStep = nameof(options.ContentFolder);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(options.PreviewFile) && !SetItemPreview(updateHandle, options.PreviewFile))
            {
                failedStep = nameof(options.PreviewFile);
                return false;
            }

            return true;
        }

        [Documentation(nameof(UpdateItem), "Convenience method that starts an item update, applies option fields, and submits the update in one call.", new[]
        {
            "consumerAppId: Your game AppId.",
            "publishedFileId: Existing workshop item ID.",
            "options: Options object containing fields to update.",
            "changeNote: Changelog note for submission.",
            "failedStep: Name of the failed step when the call returns 0."
        }, null)]
        public ulong UpdateItem(uint consumerAppId, ulong publishedFileId, SteamUgcItemUpdateOptions options, string changeNote, out string failedStep)
        {
            failedStep = string.Empty;

            var updateHandle = StartItemUpdate(consumerAppId, publishedFileId);
            if (updateHandle == 0)
            {
                failedStep = nameof(StartItemUpdate);
                return 0;
            }

            if (!ApplyItemUpdate(updateHandle, options, out failedStep))
            {
                return 0;
            }

            var submitHandle = SubmitItemUpdate(updateHandle, changeNote);
            if (submitHandle == 0)
            {
                failedStep = nameof(SubmitItemUpdate);
            }

            return submitHandle;
        }

        [Documentation(nameof(CreateItem), "Creates a new Workshop item and returns a Steam API call handle.", new[]
        {
            "consumerAppId: Your game AppId.",
            "fileType: Numeric EWorkshopFileType value."
        }, null)]
        public ulong CreateItem(uint consumerAppId, uint fileType)
        {
            return SteamNativeApi.CreateItem(consumerAppId, fileType);
        }

        [Documentation(nameof(StartItemUpdate), "Starts an update transaction for an existing Workshop item.", new[]
        {
            "consumerAppId: Your game AppId.",
            "publishedFileId: Existing workshop item ID."
        }, null)]
        public ulong StartItemUpdate(uint consumerAppId, ulong publishedFileId)
        {
            return SteamNativeApi.StartItemUpdate(consumerAppId, publishedFileId);
        }

        [Documentation(nameof(SetItemTitle), "Sets workshop item title for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "title: New item title." }, null)]
        public bool SetItemTitle(ulong updateHandle, string title)
        {
            return SteamNativeApi.SetItemTitle(updateHandle, title);
        }

        [Documentation(nameof(SetItemDescription), "Sets workshop item description for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "description: New item description." }, null)]
        public bool SetItemDescription(ulong updateHandle, string description)
        {
            return SteamNativeApi.SetItemDescription(updateHandle, description);
        }

        [Documentation(nameof(SetItemMetadata), "Sets workshop item metadata for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "metadata: Metadata string (up to Steam's size limits)." }, null)]
        public bool SetItemMetadata(ulong updateHandle, string metadata)
        {
            return SteamNativeApi.SetItemMetadata(updateHandle, metadata);
        }

        [Documentation(nameof(SetItemVisibility), "Sets workshop item visibility for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "visibility: Numeric ERemoteStoragePublishedFileVisibility value." }, null)]
        public bool SetItemVisibility(ulong updateHandle, uint visibility)
        {
            return SteamNativeApi.SetItemVisibility(updateHandle, visibility);
        }

        [Documentation(nameof(SetItemTags), "Sets workshop item tags for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "tags: Array of tag strings." }, null)]
        public bool SetItemTags(ulong updateHandle, string[] tags)
        {
            return SteamNativeApi.SetItemTags(updateHandle, tags);
        }

        [Documentation(nameof(SetItemContent), "Sets workshop item content folder path for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "contentFolder: Absolute path to content folder." }, null)]
        public bool SetItemContent(ulong updateHandle, string contentFolder)
        {
            return SteamNativeApi.SetItemContent(updateHandle, contentFolder);
        }

        [Documentation(nameof(SetItemPreview), "Sets workshop item preview image path for a pending update.", new[] { "updateHandle: Handle from StartItemUpdate.", "previewFile: Absolute path to preview image file." }, null)]
        public bool SetItemPreview(ulong updateHandle, string previewFile)
        {
            return SteamNativeApi.SetItemPreview(updateHandle, previewFile);
        }

        [Documentation(nameof(SubmitItemUpdate), "Submits a pending workshop item update and returns a Steam API call handle.", new[] { "updateHandle: Handle from StartItemUpdate.", "changeNote: Changelog note for the update." }, null)]
        public ulong SubmitItemUpdate(ulong updateHandle, string changeNote)
        {
            return SteamNativeApi.SubmitItemUpdate(updateHandle, changeNote);
        }

        [Documentation(nameof(GetItemUpdateProgress), "Gets progress state for a pending item update upload.", new[] { "updateHandle: Handle from StartItemUpdate.", "bytesProcessed: Uploaded bytes output.", "bytesTotal: Total bytes output." }, null)]
        public uint GetItemUpdateProgress(ulong updateHandle, out ulong bytesProcessed, out ulong bytesTotal)
        {
            return SteamNativeApi.GetItemUpdateProgress(updateHandle, out bytesProcessed, out bytesTotal);
        }

        [Documentation(nameof(CreateQueryAllUgcRequest), "Creates a UGC query request handle for workshop discovery.", new[]
        {
            "queryType: Numeric EUGCQuery value from Steamworks docs.",
            "matchingType: Numeric EUGCMatchingUGCType value from Steamworks docs.",
            "creatorAppId: AppId filter for creator app.",
            "consumerAppId: AppId filter for consumer app.",
            "page: Query page number, starting at 1."
        }, null)]
        public ulong CreateQueryAllUgcRequest(uint queryType, uint matchingType, uint creatorAppId, uint consumerAppId, uint page = 1)
        {
            return SteamNativeApi.CreateQueryAllUgcRequest(queryType, matchingType, creatorAppId, consumerAppId, page);
        }

        [Documentation(nameof(SendQueryUgcRequest), "Submits an existing UGC query request and returns a Steam API call handle.", new[] { "queryHandle: UGC query handle returned from CreateQueryAllUgcRequest." }, null)]
        public ulong SendQueryUgcRequest(ulong queryHandle)
        {
            return SteamNativeApi.SendQueryUgcRequest(queryHandle);
        }

        [Documentation(nameof(ReleaseQueryUgcRequest), "Releases a UGC query handle after you are done with query results.", new[] { "queryHandle: UGC query handle to release." }, null)]
        public bool ReleaseQueryUgcRequest(ulong queryHandle)
        {
            return SteamNativeApi.ReleaseQueryUgcRequest(queryHandle);
        }

        [Documentation(nameof(GetNumSubscribedItems), "Gets the number of workshop items the current user is subscribed to.", null, null)]
        public uint GetNumSubscribedItems()
        {
            return SteamNativeApi.GetNumSubscribedItems();
        }

        [Documentation(nameof(GetSubscribedItems), "Gets subscribed workshop item IDs for the current user.", new[] { "maxEntries: Maximum number of subscribed IDs to retrieve." }, null)]
        public ulong[] GetSubscribedItems(uint maxEntries = 1024)
        {
            return SteamNativeApi.GetSubscribedItems(maxEntries);
        }

        [Documentation(nameof(SubscribeItem), "Subscribes the current user to a workshop item.", new[] { "publishedFileId: Published file ID of the workshop item." }, null)]
        public ulong SubscribeItem(ulong publishedFileId)
        {
            return SteamNativeApi.SubscribeItem(publishedFileId);
        }

        [Documentation(nameof(UnsubscribeItem), "Unsubscribes the current user from a workshop item.", new[] { "publishedFileId: Published file ID of the workshop item." }, null)]
        public ulong UnsubscribeItem(ulong publishedFileId)
        {
            return SteamNativeApi.UnsubscribeItem(publishedFileId);
        }
    }
}

