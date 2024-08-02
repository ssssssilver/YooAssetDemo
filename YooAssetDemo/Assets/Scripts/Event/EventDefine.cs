using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;


public interface IEventMessage
{
}
public class EventDefine
{
    /// <summary>
    /// 补丁包初始化
    /// </summary>
    public class UserInitlize : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserInitlize();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 初始化失败
    /// </summary>
    public class InitlizeFail : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new InitlizeFail();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 补丁流程改变
    /// </summary>
    public class PatchStateChange : IEventMessage
    {
        public string tips;
        public static void SendEventMessage(string tips)
        {
            var msg = new PatchStateChange();
            msg.tips = tips;
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 开始下载
    /// </summary>
    public class BeginDownload : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new BeginDownload();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 更新补丁包版本
    /// </summary>
    public class UpdatePackageVersion : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UpdatePackageVersion();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 更新补丁清单
    /// </summary>
    public class UpdatePatchMainifest : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UpdatePatchMainifest();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 下载文件
    /// </summary>
    public class DownloadFile : IEventMessage
    {
        public string fileName;
        public DownloadFile(string fileName)
        {
            this.fileName = fileName;
        }
        public static void SendEventMessage(string fileName)
        {
            var msg = new DownloadFile(fileName);
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 资源版本号更新失败
    /// </summary>
    public class PackageVersionUpdateFailed : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new PackageVersionUpdateFailed();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 补丁清单更新失败
    /// </summary>
    public class PatchManifestUpdateFailed : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new PatchManifestUpdateFailed();
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 网络文件下载失败
    /// </summary>
    public class WebFileDownloadFailed : IEventMessage
    {
        public string FileName;
        public string Error;

        public static void SendEventMessage(string fileName, string error)
        {
            var msg = new WebFileDownloadFailed();
            msg.FileName = fileName;
            msg.Error = error;
            EventGroup.SendMessage(msg);
        }
    }

    /// <summary>
    /// 发现更新文件
    /// </summary>
    public class FoundUpdateFiles : IEventMessage
    {

        public int TotalCount;
        public long TotalSizeBytes;
        public string version;

        public static void SendEventMessage(string version, int totalCount, long totalSizeBytes)
        {
            var msg = new FoundUpdateFiles();
            msg.version = version;
            msg.TotalCount = totalCount;
            msg.TotalSizeBytes = totalSizeBytes;
            EventGroup.SendMessage(msg);
        }
    }

    /// <summary>
    /// 下载进度更新
    /// </summary>
    public class DownloadProgressUpdate : IEventMessage
    {
        public int TotalDownloadCount;
        public int CurrentDownloadCount;
        public long TotalDownloadSizeBytes;
        public long CurrentDownloadSizeBytes;

        public static void SendEventMessage(int totalDownloadCount, int currentDownloadCount, long totalDownloadSizeBytes, long currentDownloadSizeBytes)
        {
            var msg = new DownloadProgressUpdate();
            msg.TotalDownloadCount = totalDownloadCount;
            msg.CurrentDownloadCount = currentDownloadCount;
            msg.TotalDownloadSizeBytes = totalDownloadSizeBytes;
            msg.CurrentDownloadSizeBytes = currentDownloadSizeBytes;
            EventGroup.SendMessage(msg);
        }


    }
    /// <summary>
    /// 更新完成 
    /// </summary>
    public class PatchProcessDone : IEventMessage
    {
        public bool needUpdate;
        public static void SendEventMessage(bool needUpdate)
        {
            var msg = new PatchProcessDone();
            msg.needUpdate = needUpdate;
            EventGroup.SendMessage(msg);
        }
    }
    /// <summary>
    /// 退出更新流程
    /// </summary>
    public class PatchProcessExit : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new PatchProcessExit();
            EventGroup.SendMessage(msg);
        }
    }



}
