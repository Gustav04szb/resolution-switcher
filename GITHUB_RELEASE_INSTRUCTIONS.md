# Anleitung zum Veröffentlichen der Resolution Switcher App auf GitHub

## 1. GitHub-Repository erstellen

1. Besuchen Sie [GitHub](https://github.com) und melden Sie sich an
2. Klicken Sie auf das "+"-Symbol in der oberen rechten Ecke und wählen Sie "New repository"
3. Geben Sie "Resolution-Switcher" als Repository-Namen ein
4. Fügen Sie eine kurze Beschreibung hinzu: "Eine einfache Windows 11 App zum Wechseln der Bildschirmauflösung"
5. Lassen Sie das Repository öffentlich
6. Wählen Sie KEINE Option zum Initialisieren des Repositories (kein README, keine .gitignore, keine Lizenz)
7. Klicken Sie auf "Create repository"

## 2. Verbinden Sie Ihr lokales Repository mit GitHub

Führen Sie folgende Befehle in PowerShell oder CMD im Projektverzeichnis aus:

```powershell
git remote add origin https://github.com/IhrBenutzername/Resolution-Switcher.git
git branch -M main
git push -u origin main
```

Ersetzen Sie `IhrBenutzername` durch Ihren GitHub-Benutzernamen.

## 3. Release erstellen

1. Überprüfen Sie Ihren Code auf GitHub im Browser
2. Klicken Sie auf "Releases" in der rechten Seitenleiste
3. Klicken Sie auf "Create a new release"
4. Geben Sie "v1.0.0" als Tag Version ein
5. Geben Sie "Resolution Switcher - Initial Release" als Release-Titel ein
6. Fügen Sie eine Beschreibung hinzu:

```
# Resolution Switcher

Eine einfache Windows 11 App zum Wechseln der Bildschirmauflösung.

## Features
- Wechseln zu 1440x1080 Auflösung mit einem Klick
- Zurücksetzen zur maximalen Bildschirmauflösung
- Moderne Windows 11 UI mit benutzerdefinierten Farben

## Installationsanweisungen
1. Laden Sie die Zip-Datei herunter und entpacken Sie sie
2. Führen Sie die Strecher.exe aus
```

7. Ziehen Sie die "Strecher_x64_selfcontained_fixed.zip" Datei in den Bereich "Attach binaries by dropping them here..."
8. Aktivieren Sie die Option "This is a pre-release" wenn dies eine Beta-Version ist
9. Klicken Sie auf "Publish release"

## 4. Aktualisieren der README.md

Nachdem der Release erstellt wurde, können Sie die README.md aktualisieren, um den Link zum neuesten Release hinzuzufügen. 