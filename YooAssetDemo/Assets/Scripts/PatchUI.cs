using UniFramework.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EventDefine;

public class PatchUI : MonoBehaviour
{
    [SerializeField]
    private Text content;
    [SerializeField]
    private Text msgContent;
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject messageBox;
    [SerializeField]
    private Button confirm;
    [SerializeField]
    private Text patchVersion;

    UnityAction confirmAction;

    public void SetVersion(string version)
    {
        this.patchVersion.text = version;
    }

    private void Awake()
    {
        this.AddListener<InitlizeFail>(OnInitlizeFail);
        this.AddListener<PatchStateChange>(OnPatchStateChange);
        this.AddListener<FoundUpdateFiles>(OnFoundUpdateFiles);
        this.AddListener<DownloadProgressUpdate>(OnUpdateProgress);
        this.AddListener<PackageVersionUpdateFailed>(OnPackageVersionUpdateFailed);
        this.AddListener<PatchManifestUpdateFailed>(OnPatchManifestUpdateFailed);
        this.AddListener<WebFileDownloadFailed>(OnWebFileDownloadFailed);
        this.AddListener<PatchProcessDone>(OnPatchProcessDone);
        confirm.onClick.AddListener(OnClickConfirm);
    }

    void ShowMessageBox(string msg, UnityAction action, string confirmText = "确定")
    {
        msgContent.text = msg;
        confirm.GetComponentInChildren<Text>().text = confirmText;
        confirmAction = action;
        messageBox.SetActive(true);
    }

    void OnClickConfirm()
    {
        content.text = "";
        confirmAction?.Invoke();
        messageBox.SetActive(false);
    }

    int initTime = 0;
    int initTotalTime = 3;
    void OnInitlizeFail(InitlizeFail initlizeFail)
    {
        initTime++;
        if (initTime >= initTotalTime)
        {
            ShowMessageBox("初始化失败,请确定网络无异常后再重试", () =>
                        {

                        });
        }
        else
        {
            ShowMessageBox("初始化失败", () =>
            {
                UserInitlize.SendEventMessage();
            }, "重试");
        }
    }
    void OnPackageVersionUpdateFailed(PackageVersionUpdateFailed packageVersionUpdateFailed)
    {
        ShowMessageBox("获取版本号失败，请检查网络", () =>
        {
            UpdatePackageVersion.SendEventMessage();
        });
    }
    void OnPatchManifestUpdateFailed(PatchManifestUpdateFailed patchManifestUpdateFailed)
    {
        ShowMessageBox("更新清单失败，请检查网络", () =>
        {
            UpdatePatchMainifest.SendEventMessage();
        });
    }
    void OnWebFileDownloadFailed(WebFileDownloadFailed webFileDownloadFailed)
    {
        ShowMessageBox($"文件{webFileDownloadFailed.FileName}下载失败，请检查网络", () =>
        {
            DownloadFile.SendEventMessage(webFileDownloadFailed.FileName);
        });
    }
    void OnPatchStateChange(PatchStateChange patchStateChange)
    {
        content.text = patchStateChange.tips;
    }
    void OnFoundUpdateFiles(FoundUpdateFiles foundUpdateFiles)
    {
        float sizeMB = foundUpdateFiles.TotalSizeBytes / 1024.0f / 1024.0f;
        sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
        string totalSizeMb = sizeMB.ToString("F2");
        ShowMessageBox($"发现新版本:{foundUpdateFiles.version},更新文件{foundUpdateFiles.TotalCount}个,总大小{totalSizeMb}MB", () =>
        {
            BeginDownload.SendEventMessage();
        });
    }
    void OnUpdateProgress(DownloadProgressUpdate downloadProgressUpdate)
    {
        slider.value = downloadProgressUpdate.CurrentDownloadCount / (float)downloadProgressUpdate.TotalDownloadCount;
        string currentSizeBytes = (downloadProgressUpdate.CurrentDownloadSizeBytes / 1024f / 1024f).ToString("F2");
        string totalSizeBytes = (downloadProgressUpdate.TotalDownloadSizeBytes / 1024f / 1024f).ToString("F2");
        content.text = $"{downloadProgressUpdate.CurrentDownloadCount}/{downloadProgressUpdate.TotalDownloadCount}下载中...{currentSizeBytes}MB/{totalSizeBytes}MB";
    }

    void OnPatchProcessDone(PatchProcessDone patchProcessDone)
    {
        if (!patchProcessDone.needUpdate)
        {
            Debug.Log("资源包已经是最新版本");
            PatchProcessExit.SendEventMessage();
        }
        else
        {
            ShowMessageBox("更新完成", () =>
            {
                Debug.Log("更新完成");
                PatchProcessExit.SendEventMessage();
            });
        }
    }

}
