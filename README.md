# DriveMapper
*A lightweight Windows utility for secure, one-click network drive connections.*

*CURRENTLY IN DEVELOPMENT*

---

## 🧭 Overview
**DriveMapper** is a simple Windows application that helps users connect to shared network drives without needing to remember UNC paths or use command-line tools.  
It provides a straightforward interface for end users and an admin mode for IT staff to manage shared drive profiles.  
DriveMapper includes optional integration with **Windows Credential Manager** for securely storing and retrieving credentials.

---

## ✨ Features
- **One-click mapping** — Choose a saved profile and connect instantly.  
- **Custom profiles** — IT admins can define approved UNC paths and drive letters.  
- **User authentication** — Supports domain or local credentials with secure credential handling.  
- **Credential Manager integration** — Option to securely store and reuse credentials through Windows Credential Manager.  
- **Central configuration** — Profiles stored at `%ProgramData%\DriveMapper\Profiles.json`.  
- **Event logging** — Writes activity and errors to the Windows Event Log (with file fallback).  
- **Admin mode** — “IT Admins” group members can add, edit, or test profiles.  
- **Security-aware** — No plaintext credential storage; respects Windows ACLs and permissions.

---

## 🧰 Installation
1. Download the latest installer from the **Releases** page.  
2. Run the installer as an administrator.  
3. Profiles are stored in:
   ```
   %ProgramData%\DriveMapper\Profiles.json
   ```
4. Default permissions:
   - **Administrators:** Modify access  
   - **Users:** Read-only access

---

## 🖥️ Usage
### For regular users
1. Open **DriveMapper**.  
2. Select a profile from the dropdown list.  
3. Enter credentials if required (`DOMAIN\user` or local).  
4. Choose whether to save the credentials in Windows Credential Manager for future sessions.  
5. Click **Connect** — the drive appears in File Explorer.  
6. Use **Disconnect** to unmap when finished.

### For Administrators
1. Click **Admin** to open the management panel.  
2. Add or edit drive profiles (`Name`, `UNC Path`, `Drive Letter`).  
3. Use **Test** to validate share reachability.  
4. Click **Save** to update the system-wide configuration.  
5. Credentials saved in Credential Manager can be managed per user or per system policy.

---

## 🧱 Technical details
- **Language:** Visual Basic (.NET 8)  
- **Framework:** Windows Forms  
- **Config:** JSON under `%ProgramData%`  
- **Credential storage:** Windows Credential Manager integration for encrypted credential handling  
- **Network calls:** `WNetAddConnection2` / `WNetCancelConnection2` (Win32 API)  
- **Logging:** Windows Event Log + file fallback  
- **Installer:** MSIX / MSI with code signing

---

## 🧑‍💻 Development
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/DriveMapper.git
   ```
2. Open `DriveMapper.sln` in **Visual Studio 2022** or later.  
3. Build the solution in **Release** mode.  
4. Executable output: `bin\Release\net8.0-windows\DriveMapper.exe`

---

## 📄 License
This project is licensed under the **Creative Commons Attribution-NonCommercial 4.0 International License (CC BY-NC 4.0)**.  
You may use, modify, and share this software for **non-commercial** purposes with proper attribution.

See the [LICENSE](LICENSE) file for full terms.

---

## 🤝 Contributing
Pull requests are welcome for bug fixes, small improvements, and documentation updates.  
Please test any changes on Windows 10 and 11 before submitting a PR.

---

## 🧩 Author
**Drew Schmidt**  
Built for IT administrators and teams who want a simple, secure way to manage shared drives, and allow their users an easy to use interface.

---

*DriveMapper — Secure. Simple. Share connected.*
