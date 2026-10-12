using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace HDDKeepAlive
{
    public static class I18n
    {
        public static string CurrentLanguage = "zh"; // "zh" or "en"

        public static string T(string key, params object[] args)
        {
            string val;
            Dictionary<string, string> dict = (CurrentLanguage == "en") ? StringsEn : StringsZh;
            if (!dict.TryGetValue(key, out val))
            {
                if (!StringsZh.TryGetValue(key, out val))
                {
                    val = key;
                }
            }
            if (args != null && args.Length > 0)
            {
                try { return string.Format(val, args); } catch { return val; }
            }
            return val;
        }

        private static readonly Dictionary<string, string> StringsZh = new Dictionary<string, string>
        {
            { "AppTitle", "外接硬盘防休眠助手" },
            { "AppRunningAlready", "外接硬盘防休眠助手已在后台运行中。\n您可以在任务栏右下角系统托盘找到它。" },
            { "MenuTitle", "外接硬盘防休眠助手 v2.1" },
            { "DetectingDisks", "正在检测外接存储..." },
            { "ProtectingPrefix", "守护中: " },
            { "StatusNone", "状态: 暂无受保护的外接盘" },
            { "TrayTooltipStandby", "外接硬盘防休眠助手 (待命中)" },
            { "TrayTooltipProtecting", "外接硬盘防休眠助手 ({0})" },
            { "MenuDisks", "外接磁盘管理 (点击切换勾选)" },
            { "MenuDisksNone", "未检测到外接 USB 驱动器" },
            { "MenuKeepNow", "立即执行心跳保活" },
            { "MenuInterval", "心跳频率设置" },
            { "Interval30", "30 秒" },
            { "Interval60", "60 秒 (推荐)" },
            { "Interval120", "120 秒" },
            { "Interval180", "180 秒" },
            { "MenuAutoStart", "开机自动启动" },
            { "MenuLanguage", "界面语言 / Language" },
            { "LangZh", "简体中文 (Chinese)" },
            { "LangEn", "English" },
            { "MenuStats", "运行统计" },
            { "MenuExit", "退出程序" },
            { "OpenInExplorer", "在资源管理器中打开" },
            { "LocalDiskDefault", "本地磁盘" },
            { "UsbDeviceDefault", "USB 存储设备" },
            { "BalloonReadyTitle", "外接硬盘防休眠助手已就绪" },
            { "BalloonReadyContent", "已自适应识别 USB 存储设备：{0}\n盘片将保持平稳运行，彻底消除唤醒等待。" },
            { "BalloonHeartbeatDoneTitle", "心跳完成" },
            { "BalloonHeartbeatDoneContent", "已向所有受保护的外接磁盘刷新硬件缓存。" },
            { "BalloonIntervalUpdatedTitle", "频率已更新" },
            { "BalloonIntervalUpdatedContent", "已将心跳刷新间隔调整为 {0} 秒。" },
            { "BalloonAutoStartTitle", "自启动设置" },
            { "BalloonAutoStartEnabled", "已开启开机自动静默启动！" },
            { "BalloonAutoStartDisabled", "已关闭开机自启。" },
            { "AutoStartError", "设置自启动失败: " },
            { "StatsTitle", "运行统计 - 外接硬盘防休眠助手" },
            { "StatsContent", "运行时间: {0} 小时 {1} 分钟\n累计保护心跳: {2} 次\n当前受保护分区: {3}\n刷新频率: 每 {4} 秒一次\n\n提示: 程序正在静默维持外接硬盘转速，防止马达停转。" },
            { "None", "无" }
        };

        private static readonly Dictionary<string, string> StringsEn = new Dictionary<string, string>
        {
            { "AppTitle", "External HDD Keep-Alive" },
            { "AppRunningAlready", "External HDD Keep-Alive is already running in background.\nYou can find it in the system tray." },
            { "MenuTitle", "HDD Keep-Alive v2.1" },
            { "DetectingDisks", "Detecting external drives..." },
            { "ProtectingPrefix", "Active: " },
            { "StatusNone", "Status: No active external disks" },
            { "TrayTooltipStandby", "HDD Keep-Alive (Standby)" },
            { "TrayTooltipProtecting", "HDD Keep-Alive ({0})" },
            { "MenuDisks", "External Disks (Click to toggle)" },
            { "MenuDisksNone", "No external USB disks detected" },
            { "MenuKeepNow", "Keep Alive Now" },
            { "MenuInterval", "Keep-Alive Interval" },
            { "Interval30", "30 Seconds" },
            { "Interval60", "60 Seconds (Recommended)" },
            { "Interval120", "120 Seconds" },
            { "Interval180", "180 Seconds" },
            { "MenuAutoStart", "Start with Windows" },
            { "MenuLanguage", "Language / 语言" },
            { "LangZh", "简体中文 (Chinese)" },
            { "LangEn", "English" },
            { "MenuStats", "Statistics" },
            { "MenuExit", "Exit" },
            { "OpenInExplorer", "Open in File Explorer" },
            { "LocalDiskDefault", "Local Disk" },
            { "UsbDeviceDefault", "USB Storage Device" },
            { "BalloonReadyTitle", "HDD Keep-Alive Ready" },
            { "BalloonReadyContent", "Detected USB storage devices: {0}\nDisks will stay awake to eliminate spin-up delay." },
            { "BalloonHeartbeatDoneTitle", "Heartbeat Sent" },
            { "BalloonHeartbeatDoneContent", "Flushed hardware cache for all active USB disks." },
            { "BalloonIntervalUpdatedTitle", "Interval Updated" },
            { "BalloonIntervalUpdatedContent", "Keep-alive interval updated to {0} seconds." },
            { "BalloonAutoStartTitle", "Auto-Start Setting" },
            { "BalloonAutoStartEnabled", "Enabled auto-start with Windows." },
            { "BalloonAutoStartDisabled", "Disabled auto-start with Windows." },
            { "AutoStartError", "Failed to configure auto-start: " },
            { "StatsTitle", "Statistics - HDD Keep-Alive" },
            { "StatsContent", "Uptime: {0} hr {1} min\nTotal heartbeats: {2}\nActive drives: {3}\nInterval: Every {4} seconds\n\nNote: Keeping disk platters spinning silently." },
            { "None", "None" }
        };
    }

    static class Program
    {
        private static Mutex mutex;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 预读取注册表语言偏好以决定单例提示的语言
            InitLanguagePreference();

            bool createdNew;
            mutex = new Mutex(true, "Global\\HDDKeepAlive_Singleton_Mutex", out createdNew);
            if (!createdNew)
            {
                MessageBox.Show(I18n.T("AppRunningAlready"),
                    I18n.T("AppTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Application.Run(new TrayApplicationContext());
            }
            catch (Exception ex)
            {
                try { File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log"), ex.ToString()); } catch { }
            }
            GC.KeepAlive(mutex);
        }

        private static void InitLanguagePreference()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\HDDKeepAlive", false))
                {
                    if (key != null)
                    {
                        object langVal = key.GetValue("Language");
                        if (langVal != null)
                        {
                            string l = langVal.ToString().ToLowerInvariant();
                            if (l == "zh" || l == "en")
                            {
                                I18n.CurrentLanguage = l;
                                return;
                            }
                        }
                    }
                }
            }
            catch { }

            // 默认探测系统 UI 语言
            try
            {
                string sys = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                I18n.CurrentLanguage = sys.Equals("zh", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
            }
            catch
            {
                I18n.CurrentLanguage = "zh";
            }
        }
    }

    public class DiskInfoItem
    {
        public string DriveLetter { get; set; } // e.g. "E:\"
        public string VolumeLabel { get; set; }
        public string FileSystem { get; set; }
        public string DiskModel { get; set; }
        public long TotalFreeSpaceBytes { get; set; }
        public long TotalSizeBytes { get; set; }
        public bool IsEnabled { get; set; }

        public string DisplayText
        {
            get
            {
                string letter = DriveLetter.TrimEnd('\\');
                string label = string.IsNullOrEmpty(VolumeLabel) ? I18n.T("LocalDiskDefault") : VolumeLabel;
                string freeGb = (TotalFreeSpaceBytes / (1024.0 * 1024 * 1024)).ToString("0.#");
                string totalGb = (TotalSizeBytes / (1024.0 * 1024 * 1024)).ToString("0.#");
                return string.Format("[{0}] {1} ({2} GB / {3} GB)", letter, label, freeGb, totalGb);
            }
        }
    }

    public class TrayApplicationContext : ApplicationContext
    {
        private const string RegKeyPath = @"Software\HDDKeepAlive";
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "HDDKeepAlive";

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private System.Windows.Forms.Timer keepAliveTimer;
        private DeviceChangeReceiver deviceReceiver;

        private int intervalSeconds = 60;
        private long heartbeatCount = 0;
        private DateTime startTime;
        private Dictionary<string, DiskInfoItem> currentDisks = new Dictionary<string, DiskInfoItem>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> userDisabledDrives = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 菜单项引用
        private ToolStripMenuItem titleItem;
        private ToolStripMenuItem statusMenuItem;
        private ToolStripMenuItem disksMenu;
        private ToolStripMenuItem keepNowItem;
        private ToolStripMenuItem intervalMenu;
        private ToolStripMenuItem interval30Item;
        private ToolStripMenuItem interval60Item;
        private ToolStripMenuItem interval120Item;
        private ToolStripMenuItem interval180Item;
        private ToolStripMenuItem autoStartMenuItem;
        private ToolStripMenuItem languageMenu;
        private ToolStripMenuItem langZhItem;
        private ToolStripMenuItem langEnItem;
        private ToolStripMenuItem statsItem;
        private ToolStripMenuItem exitItem;

        public TrayApplicationContext()
        {
            startTime = DateTime.Now;

            LoadSettings();
            InitializeTray();

            // 监听硬件热插拔消息
            deviceReceiver = new DeviceChangeReceiver(this);

            // 执行初始扫描和首次心跳
            ScanDisks(true);
            PerformHeartbeat();

            // 启动定时器
            keepAliveTimer = new System.Windows.Forms.Timer();
            keepAliveTimer.Interval = intervalSeconds * 1000;
            keepAliveTimer.Tick += (s, e) => PerformHeartbeat();
            keepAliveTimer.Start();
        }

        private void InitializeTray()
        {
            contextMenu = new ContextMenuStrip();
            contextMenu.Font = new Font("Segoe UI", 9F);

            // 标题
            titleItem = new ToolStripMenuItem();
            titleItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            titleItem.Enabled = false;
            contextMenu.Items.Add(titleItem);
            contextMenu.Items.Add(new ToolStripSeparator());

            // 状态
            statusMenuItem = new ToolStripMenuItem();
            statusMenuItem.Enabled = false;
            contextMenu.Items.Add(statusMenuItem);

            // 保护的磁盘列表子菜单
            disksMenu = new ToolStripMenuItem();
            contextMenu.Items.Add(disksMenu);

            // 立即保活
            keepNowItem = new ToolStripMenuItem(string.Empty, null, (s, e) =>
            {
                PerformHeartbeat();
                notifyIcon.ShowBalloonTip(2000, I18n.T("BalloonHeartbeatDoneTitle"),
                    I18n.T("BalloonHeartbeatDoneContent"), ToolTipIcon.Info);
            });
            contextMenu.Items.Add(keepNowItem);

            contextMenu.Items.Add(new ToolStripSeparator());

            // 频率设置
            intervalMenu = new ToolStripMenuItem();
            interval30Item = new ToolStripMenuItem(string.Empty, null, (s, e) => SetInterval(30));
            interval60Item = new ToolStripMenuItem(string.Empty, null, (s, e) => SetInterval(60));
            interval120Item = new ToolStripMenuItem(string.Empty, null, (s, e) => SetInterval(120));
            interval180Item = new ToolStripMenuItem(string.Empty, null, (s, e) => SetInterval(180));
            intervalMenu.DropDownItems.AddRange(new ToolStripItem[] { interval30Item, interval60Item, interval120Item, interval180Item });
            UpdateIntervalMenuChecks();
            contextMenu.Items.Add(intervalMenu);

            // 开机自启
            autoStartMenuItem = new ToolStripMenuItem(string.Empty, null, (s, e) => ToggleAutoStart());
            autoStartMenuItem.Checked = IsAutoStartEnabled();
            contextMenu.Items.Add(autoStartMenuItem);

            // 语言选项
            languageMenu = new ToolStripMenuItem();
            langZhItem = new ToolStripMenuItem(string.Empty, null, (s, e) => SwitchLanguage("zh"));
            langEnItem = new ToolStripMenuItem(string.Empty, null, (s, e) => SwitchLanguage("en"));
            languageMenu.DropDownItems.Add(langZhItem);
            languageMenu.DropDownItems.Add(langEnItem);
            UpdateLanguageMenuChecks();
            contextMenu.Items.Add(languageMenu);

            // 统计信息
            statsItem = new ToolStripMenuItem(string.Empty, null, (s, e) => ShowStatistics());
            contextMenu.Items.Add(statsItem);

            contextMenu.Items.Add(new ToolStripSeparator());

            // 退出
            exitItem = new ToolStripMenuItem(string.Empty, null, (s, e) =>
            {
                keepAliveTimer.Stop();
                if (deviceReceiver != null) deviceReceiver.DestroyHandle();
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                Application.Exit();
            });
            contextMenu.Items.Add(exitItem);

            // NotifyIcon 初始化
            notifyIcon = new NotifyIcon();
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Icon = CreateHddIcon(true);
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += (s, e) => PerformHeartbeat();

            // 初始化文本
            UpdateMenuTexts();
        }

        private void UpdateMenuTexts()
        {
            titleItem.Text = I18n.T("MenuTitle");
            disksMenu.Text = I18n.T("MenuDisks");
            keepNowItem.Text = I18n.T("MenuKeepNow");
            intervalMenu.Text = I18n.T("MenuInterval");
            interval30Item.Text = I18n.T("Interval30");
            interval60Item.Text = I18n.T("Interval60");
            interval120Item.Text = I18n.T("Interval120");
            interval180Item.Text = I18n.T("Interval180");
            autoStartMenuItem.Text = I18n.T("MenuAutoStart");
            languageMenu.Text = I18n.T("MenuLanguage");
            langZhItem.Text = I18n.T("LangZh");
            langEnItem.Text = I18n.T("LangEn");
            statsItem.Text = I18n.T("MenuStats");
            exitItem.Text = I18n.T("MenuExit");

            UpdateDisksMenu();
            UpdateStatusDisplay();
        }

        private void SwitchLanguage(string lang)
        {
            if (I18n.CurrentLanguage == lang) return;
            I18n.CurrentLanguage = lang;
            UpdateLanguageMenuChecks();
            UpdateMenuTexts();
            SaveSettings();
        }

        private void UpdateLanguageMenuChecks()
        {
            langZhItem.Checked = (I18n.CurrentLanguage == "zh");
            langEnItem.Checked = (I18n.CurrentLanguage == "en");
        }

        public void OnDeviceChangeNotification()
        {
            // 当系统检测到 USB 插拔时触发
            ScanDisks(false);
            PerformHeartbeat();
        }

        public void ScanDisks(bool isStartup)
        {
            List<DiskInfoItem> detected = DiskDetector.DetectUsbDisks();
            Dictionary<string, DiskInfoItem> newMap = new Dictionary<string, DiskInfoItem>(StringComparer.OrdinalIgnoreCase);

            foreach (DiskInfoItem item in detected)
            {
                item.IsEnabled = !userDisabledDrives.Contains(item.DriveLetter);
                newMap[item.DriveLetter] = item;
            }

            currentDisks = newMap;
            UpdateDisksMenu();
            UpdateStatusDisplay();

            if (isStartup && currentDisks.Count > 0)
            {
                List<string> protectedList = GetActiveDrives();
                string drives = protectedList.Count > 0 ? string.Join(", ", protectedList.ToArray()) : I18n.T("None");
                notifyIcon.ShowBalloonTip(3000, I18n.T("BalloonReadyTitle"),
                    I18n.T("BalloonReadyContent", drives),
                    ToolTipIcon.Info);
            }
        }

        private List<string> GetActiveDrives()
        {
            List<string> list = new List<string>();
            foreach (DiskInfoItem item in currentDisks.Values)
            {
                if (item.IsEnabled)
                {
                    list.Add(item.DriveLetter);
                }
            }
            return list;
        }

        private void UpdateDisksMenu()
        {
            disksMenu.DropDownItems.Clear();

            if (currentDisks.Count == 0)
            {
                ToolStripMenuItem emptyItem = new ToolStripMenuItem(I18n.T("MenuDisksNone"));
                emptyItem.Enabled = false;
                disksMenu.DropDownItems.Add(emptyItem);
                return;
            }

            foreach (DiskInfoItem disk in currentDisks.Values)
            {
                DiskInfoItem currentDisk = disk;
                ToolStripMenuItem item = new ToolStripMenuItem(currentDisk.DisplayText);
                item.Checked = currentDisk.IsEnabled;
                item.Click += (s, e) =>
                {
                    currentDisk.IsEnabled = !currentDisk.IsEnabled;
                    item.Checked = currentDisk.IsEnabled;
                    if (currentDisk.IsEnabled)
                    {
                        userDisabledDrives.Remove(currentDisk.DriveLetter);
                    }
                    else
                    {
                        userDisabledDrives.Add(currentDisk.DriveLetter);
                    }
                    SaveSettings();
                    UpdateStatusDisplay();
                };

                // 添加子菜单：打开文件夹
                ToolStripMenuItem openFolder = new ToolStripMenuItem(I18n.T("OpenInExplorer"), null, (s, e) =>
                {
                    try { System.Diagnostics.Process.Start(currentDisk.DriveLetter); } catch { }
                });
                item.DropDownItems.Add(openFolder);

                disksMenu.DropDownItems.Add(item);
            }
        }

        private void UpdateStatusDisplay()
        {
            if (notifyIcon == null) return;
            List<string> active = GetActiveDrives();
            if (active.Count > 0)
            {
                string text = string.Join(", ", active.ToArray());
                statusMenuItem.Text = I18n.T("ProtectingPrefix") + text;
                string tooltip = I18n.T("TrayTooltipProtecting", text);
                if (tooltip.Length > 63) tooltip = tooltip.Substring(0, 60) + "...";
                notifyIcon.Text = tooltip;
                notifyIcon.Icon = CreateHddIcon(true);
            }
            else
            {
                statusMenuItem.Text = I18n.T("StatusNone");
                notifyIcon.Text = I18n.T("TrayTooltipStandby");
                notifyIcon.Icon = CreateHddIcon(false);
            }
        }

        private void PerformHeartbeat()
        {
            try
            {
                List<string> active = GetActiveDrives();
                int successCount = 0;

                foreach (string drive in active)
                {
                    try
                    {
                        if (Directory.Exists(drive))
                        {
                            string keepPath = Path.Combine(drive, ".hdd_keepalive");

                            // 若文件已被标记隐藏，写入前先重置为普通属性以防权限冲突
                            if (File.Exists(keepPath))
                            {
                                try { File.SetAttributes(keepPath, FileAttributes.Normal); } catch { }
                            }

                            // 写入微小时间戳并执行物理硬件刷新
                            using (FileStream fs = new FileStream(keepPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                            {
                                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                fs.Write(bytes, 0, bytes.Length);
                                fs.Flush(true); // 物理刷新到设备硬件
                            }

                            // 恢复隐藏属性
                            try { File.SetAttributes(keepPath, FileAttributes.Hidden); } catch { }
                            successCount++;
                        }
                    }
                    catch
                    {
                        // 忽略偶发拔盘
                    }
                }

                if (successCount > 0)
                {
                    heartbeatCount++;
                }
            }
            catch { }
        }

        private void SetInterval(int seconds)
        {
            intervalSeconds = seconds;
            keepAliveTimer.Interval = intervalSeconds * 1000;
            UpdateIntervalMenuChecks();
            SaveSettings();
            notifyIcon.ShowBalloonTip(1500, I18n.T("BalloonIntervalUpdatedTitle"),
                I18n.T("BalloonIntervalUpdatedContent", seconds), ToolTipIcon.Info);
        }

        private void UpdateIntervalMenuChecks()
        {
            interval30Item.Checked = (intervalSeconds == 30);
            interval60Item.Checked = (intervalSeconds == 60);
            interval120Item.Checked = (intervalSeconds == 120);
            interval180Item.Checked = (intervalSeconds == 180);
        }

        private void ShowStatistics()
        {
            TimeSpan runningTime = DateTime.Now - startTime;
            List<string> active = GetActiveDrives();
            string drives = active.Count > 0 ? string.Join(", ", active.ToArray()) : I18n.T("None");
            string msg = I18n.T("StatsContent", (int)runningTime.TotalHours, runningTime.Minutes, heartbeatCount, drives, intervalSeconds);
            MessageBox.Show(msg, I18n.T("StatsTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool IsAutoStartEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RunKeyPath, false))
                {
                    if (key != null)
                    {
                        return key.GetValue(AppName) != null;
                    }
                }
            }
            catch { }
            return false;
        }

        private void ToggleAutoStart()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true))
                {
                    if (key != null)
                    {
                        if (autoStartMenuItem.Checked)
                        {
                            key.DeleteValue(AppName, false);
                            autoStartMenuItem.Checked = false;
                            notifyIcon.ShowBalloonTip(2000, I18n.T("BalloonAutoStartTitle"),
                                I18n.T("BalloonAutoStartDisabled"), ToolTipIcon.Info);
                        }
                        else
                        {
                            key.SetValue(AppName, "\"" + Application.ExecutablePath + "\"");
                            autoStartMenuItem.Checked = true;
                            notifyIcon.ShowBalloonTip(2000, I18n.T("BalloonAutoStartTitle"),
                                I18n.T("BalloonAutoStartEnabled"), ToolTipIcon.Info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(I18n.T("AutoStartError") + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSettings()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKeyPath, false))
                {
                    if (key != null)
                    {
                        object intervalVal = key.GetValue("IntervalSeconds");
                        if (intervalVal != null)
                        {
                            int val = Convert.ToInt32(intervalVal);
                            if (val == 30 || val == 60 || val == 120 || val == 180)
                            {
                                intervalSeconds = val;
                            }
                        }

                        object langVal = key.GetValue("Language");
                        if (langVal != null)
                        {
                            string l = langVal.ToString().ToLowerInvariant();
                            if (l == "zh" || l == "en")
                            {
                                I18n.CurrentLanguage = l;
                            }
                        }

                        object disabledVal = key.GetValue("DisabledDrives");
                        if (disabledVal != null)
                        {
                            string[] drives = disabledVal.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string d in drives)
                            {
                                userDisabledDrives.Add(d.Trim());
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void SaveSettings()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegKeyPath))
                {
                    if (key != null)
                    {
                        key.SetValue("IntervalSeconds", intervalSeconds, RegistryValueKind.DWord);
                        key.SetValue("Language", I18n.CurrentLanguage, RegistryValueKind.String);
                        List<string> disabledList = new List<string>(userDisabledDrives);
                        key.SetValue("DisabledDrives", string.Join(",", disabledList.ToArray()), RegistryValueKind.String);
                    }
                }
            }
            catch { }
        }

        private Icon CreateHddIcon(bool isProtecting)
        {
            using (Bitmap bmp = new Bitmap(32, 32))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // 外壳
                using (SolidBrush bodyBrush = new SolidBrush(Color.FromArgb(45, 52, 54)))
                using (Pen borderPen = new Pen(Color.FromArgb(99, 110, 114), 1.5f))
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddArc(4, 4, 6, 6, 180, 90);
                    path.AddArc(22, 4, 6, 6, 270, 90);
                    path.AddArc(22, 22, 6, 6, 0, 90);
                    path.AddArc(4, 22, 6, 6, 90, 90);
                    path.CloseFigure();

                    g.FillPath(bodyBrush, path);
                    g.DrawPath(borderPen, path);
                }

                // 盘片
                using (SolidBrush innerBrush = new SolidBrush(isProtecting ? Color.FromArgb(9, 132, 227) : Color.FromArgb(99, 110, 114)))
                {
                    g.FillEllipse(innerBrush, 9, 8, 14, 14);
                }
                using (SolidBrush centerBrush = new SolidBrush(Color.FromArgb(223, 230, 233)))
                {
                    g.FillEllipse(centerBrush, 13, 12, 6, 6);
                }

                // 状态指示灯（保护中为明亮翠绿，待命中为中性灰）
                Color ledColor = isProtecting ? Color.FromArgb(0, 184, 148) : Color.FromArgb(178, 190, 195);
                Color ledGlow = isProtecting ? Color.FromArgb(85, 239, 196) : Color.FromArgb(223, 230, 233);

                using (SolidBrush ledBrush = new SolidBrush(ledColor))
                using (SolidBrush glowBrush = new SolidBrush(ledGlow))
                {
                    g.FillEllipse(ledBrush, 20, 20, 8, 8);
                    g.FillEllipse(glowBrush, 22, 22, 4, 4);
                }

                IntPtr hIcon = bmp.GetHicon();
                return Icon.FromHandle(hIcon);
            }
        }
    }

    /// <summary>
    /// 监听 Windows 底层硬件热插拔广播（WM_DEVICECHANGE）
    /// </summary>
    public class DeviceChangeReceiver : NativeWindow
    {
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        private TrayApplicationContext context;

        public DeviceChangeReceiver(TrayApplicationContext context)
        {
            this.context = context;
            CreateParams cp = new CreateParams();
            cp.Caption = "HDDKeepAliveDeviceListener";
            this.CreateHandle(cp);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_DEVICECHANGE)
            {
                int eventType = m.WParam.ToInt32();
                if (eventType == DBT_DEVICEARRIVAL || eventType == DBT_DEVICEREMOVECOMPLETE)
                {
                    // 硬件插入或拔出，延迟 1.5 秒等待文件系统挂载完毕后刷新
                    ThreadPool.QueueUserWorkItem(delegate(object state)
                    {
                        Thread.Sleep(1500);
                        if (context != null)
                        {
                            context.OnDeviceChangeNotification();
                        }
                    });
                }
            }
        }
    }

    /// <summary>
    /// 全能自适应磁盘检测器（双引擎：Win32 IOCTL 优先 + Storage WMI 补充）
    /// </summary>
    public static class DiskDetector
    {
        private const uint FILE_SHARE_READ = 1;
        private const uint FILE_SHARE_WRITE = 2;
        private const uint OPEN_EXISTING = 3;
        private const uint IOCTL_STORAGE_QUERY_PROPERTY = 0x002D1400;

        [StructLayout(LayoutKind.Sequential)]
        struct STORAGE_PROPERTY_QUERY
        {
            public uint PropertyId;
            public uint QueryType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] AdditionalParameters;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct STORAGE_DEVICE_DESCRIPTOR
        {
            public uint Version;
            public uint Size;
            public byte DeviceType;
            public byte DeviceTypeModifier;
            public byte RemovableMedia;
            public byte CommandQueueing;
            public uint VendorIdOffset;
            public uint ProductIdOffset;
            public uint ProductRevisionOffset;
            public uint SerialNumberOffset;
            public int BusType; // 7 = BusTypeUsb
            public uint RawPropertiesLength;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            ref STORAGE_PROPERTY_QUERY lpInBuffer,
            uint nInBufferSize,
            out STORAGE_DEVICE_DESCRIPTOR lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        public static List<DiskInfoItem> DetectUsbDisks()
        {
            List<DiskInfoItem> results = new List<DiskInfoItem>();
            HashSet<string> addedLetters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string systemRoot = Path.GetPathRoot(Environment.SystemDirectory).ToUpper();

            // 引擎 1：直接调用 Windows NT 内核 Win32 IOCTL（最精准、最快、零权限要求）
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (!drive.IsReady) continue;
                    if (drive.DriveType != DriveType.Fixed && drive.DriveType != DriveType.Removable) continue;

                    string driveLetter = drive.Name.ToUpper();
                    if (driveLetter.Equals(systemRoot, StringComparison.OrdinalIgnoreCase)) continue; // 坚决不碰系统盘 C:\

                    string devPath = @"\\.\" + drive.Name.Substring(0, 2);
                    using (SafeFileHandle handle = CreateFile(devPath, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
                    {
                        if (!handle.IsInvalid)
                        {
                            STORAGE_PROPERTY_QUERY query = new STORAGE_PROPERTY_QUERY();
                            query.PropertyId = 0; // StorageDeviceProperty
                            query.QueryType = 0;   // PropertyStandardQuery
                            STORAGE_DEVICE_DESCRIPTOR desc;
                            uint bytesReturned;

                            if (DeviceIoControl(handle, IOCTL_STORAGE_QUERY_PROPERTY, ref query, (uint)Marshal.SizeOf(query),
                                out desc, (uint)Marshal.SizeOf(typeof(STORAGE_DEVICE_DESCRIPTOR)), out bytesReturned, IntPtr.Zero))
                            {
                                // BusType == 7 即为 USB 总线外接驱动器
                                if (desc.BusType == 7)
                                {
                                    DiskInfoItem item = new DiskInfoItem();
                                    item.DriveLetter = driveLetter;
                                    item.VolumeLabel = drive.VolumeLabel;
                                    item.FileSystem = drive.DriveFormat;
                                    item.TotalFreeSpaceBytes = drive.AvailableFreeSpace;
                                    item.TotalSizeBytes = drive.TotalSize;
                                    item.DiskModel = I18n.T("UsbDeviceDefault");
                                    item.IsEnabled = true;

                                    addedLetters.Add(driveLetter);
                                    results.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            // 引擎 2：Storage WMI 补充扫描（若上述 IOCTL 因罕见权限未扫出，则用 WMI 补充）
            if (results.Count == 0)
            {
                try
                {
                    ManagementScope scope = new ManagementScope(@"root\Microsoft\Windows\Storage");
                    scope.Connect();

                    HashSet<uint> usbDiskNumbers = new HashSet<uint>();
                    using (ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher(scope,
                        new ObjectQuery("SELECT Number, BusType FROM MSFT_Disk WHERE BusType = 7")))
                    {
                        foreach (ManagementObject disk in diskSearcher.Get())
                        {
                            usbDiskNumbers.Add(Convert.ToUInt32(disk["Number"]));
                        }
                    }

                    if (usbDiskNumbers.Count > 0)
                    {
                        using (ManagementObjectSearcher partSearcher = new ManagementObjectSearcher(scope,
                            new ObjectQuery("SELECT DiskNumber, DriveLetter FROM MSFT_Partition WHERE DriveLetter IS NOT NULL")))
                        {
                            foreach (ManagementObject part in partSearcher.Get())
                            {
                                uint diskNum = Convert.ToUInt32(part["DiskNumber"]);
                                if (usbDiskNumbers.Contains(diskNum))
                                {
                                    object letterObj = part["DriveLetter"];
                                    if (letterObj != null)
                                    {
                                        string letter = letterObj.ToString().Trim();
                                        if (letter.Length == 1 && char.IsLetter(letter[0]))
                                        {
                                            string fullLetter = letter.ToUpper() + @":\";
                                            if (!addedLetters.Contains(fullLetter) && !fullLetter.Equals(systemRoot, StringComparison.OrdinalIgnoreCase))
                                            {
                                                try
                                                {
                                                    DriveInfo d = new DriveInfo(fullLetter);
                                                    if (d.IsReady)
                                                    {
                                                        DiskInfoItem item = new DiskInfoItem();
                                                        item.DriveLetter = fullLetter;
                                                        item.VolumeLabel = d.VolumeLabel;
                                                        item.FileSystem = d.DriveFormat;
                                                        item.TotalFreeSpaceBytes = d.AvailableFreeSpace;
                                                        item.TotalSizeBytes = d.TotalSize;
                                                        item.DiskModel = I18n.T("UsbDeviceDefault");
                                                        item.IsEnabled = true;

                                                        addedLetters.Add(fullLetter);
                                                        results.Add(item);
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }

            // 排序并返回
            results.Sort((a, b) => string.Compare(a.DriveLetter, b.DriveLetter, StringComparison.OrdinalIgnoreCase));
            return results;
        }
    }
}
