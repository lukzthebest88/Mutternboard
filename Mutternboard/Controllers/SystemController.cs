using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mutternboard.Controllers
{
    public class SystemController : Controller
    {
        [HttpGet]
        public IActionResult GetStats()
        {
            string cpu = Bash("top -bn1 | grep '%Cpu' | awk '{print $2}'");
            string usedMem = Bash("free -m | awk '/Mem:/ {print $3}'");
            string totalMem = Bash("free -m | awk '/Mem:/ {print $2}'");
            string ram = Bash("free | awk '/Mem:/ { printf(\"%.1f\", $3/$2 * 100) }'");
            string uptime = Bash("uptime -p");

            return Json(new
            {
                cpu = cpu.Trim(),
                usedMem = usedMem.Trim(),
                totalMem = totalMem.Trim(),
                ram = ram.Trim(),
                uptime = uptime.Trim()
            });
        }

        private string Bash(string cmd)
        {
            if (OperatingSystem.IsLinux())
            {

                var escapedArgs = cmd.Replace("\"", "\\\"");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result;
            }
            else
            {
                return "";
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Restart()
        {
            // Beispiel: Shell-Befehl ausführen
            if (OperatingSystem.IsLinux())
            {
                System.Diagnostics.Process.Start("bash", "-c \"sudo reboot\"");
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RestartMutternboard()
        {
            if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "-c \"sudo systemctl restart mutternboard.service\""); // Passe ggf. den Dienstnamen an
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RestartTv()
        {
            if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "-c \"sudo systemctl restart tv2.service\""); // Passe ggf. den Dienstnamen an
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult StopTv()
        {
            if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "-c \"sudo systemctl stop tv2.service\""); // Passe ggf. den Dienstnamen an
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RestartWebLgsm()
        {
            if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "-c \"sudo systemctl restart weblgsm.service\""); // Passe ggf. den Dienstnamen an
                return Ok();
            }
            return BadRequest();
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult StopWebLgsm()
        {
            if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "-c \"sudo systemctl stop weblgsm.service\""); // Passe ggf. den Dienstnamen an
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult RestartFileExchange()
        {
            if (OperatingSystem.IsLinux())
            {

                System.Diagnostics.Process.Start("docker", "restart filebrowser");
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult StopFileExchange()
        {
            if (OperatingSystem.IsLinux())
            {
                System.Diagnostics.Process.Start("docker", "stop filebrowser");
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Stop()
        {
            if (OperatingSystem.IsLinux())
            {
                System.Diagnostics.Process.Start("bash", "-c \"sudo shutdown now\"");
                return Ok();
            }
            return BadRequest();
        }
    }
}
