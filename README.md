# VolleyStats – megvalósítási jegyzet

## Felépítés

Az alkalmazás MVVM mintát használ: a `Views` csak XAML felületeket tartalmaz, a felhasználói műveletek és validációk a `ViewModels` mappában vannak, a `Models` az adatosztályokat, a `Services/DatabaseService.cs` pedig az SQLite-perzisztenciát kezeli. A `MauiProgram.cs` függőséginjektálással köti össze őket.

| Oldal | Funkció |
|---|---|
| Meccsek | mérkőzések létrehozása, listázása, módosítása, törlése |
| Játékosok | csapat választása/létrehozása, játékos CRUD csapatonként |
| Játékos profil | a kiválasztott játékos részletes adatainak és fényképének kezelése |
| Tabella | a lejátszott mérkőzésekből automatikusan számolt csapateredmények |

A harmadik oldal a játékoslistában egy játékos kiválasztásakor nyílik meg. A Shell biztosítja az első két lapos navigációt, a profiloldal pedig `GoToAsync`-szal, a kiválasztott `Player` objektummal kapja meg a navigációs paramétert.

A **Tabella** oldal csak a `Lejátszott` állapotú mérkőzéseket veszi figyelembe. Röplabda-pontozást használ: 3:0 vagy 3:1 esetén 3–0 pont, 3:2 esetén 2–1 pont. Pontegyenlőségnél a győzelmek száma, majd a szettkülönbség dönt. Emiatt az eredmény felvitelekor a két nyert szett számát is meg kell adni.

## Adattárolás

A `sqlite-net-pcl` csomag helyi `volleystats.db3` adatbázist hoz létre az alkalmazás saját adatmappájában. A `Team`, `Player` és `Match` entitások elsődleges kulcsot kapnak. Így a felvitel, szerkesztés és törlés alkalmazás-újraindítás után is megmarad. A korábbi mintaadatok szándékosan kikerültek: ez valós adatkezelést mutat.

## Kamera és megosztás

A profiloldal **Profilkép készítése kamerával** gombja a `MediaPicker.Default.CapturePhotoAsync()` API-t hívja. A kiválasztott/felvett kép az alkalmazás tartós adatkönyvtárába másolódik, az útvonala a játékos rekordjában mentődik, majd a profilban megjelenik. Ezért a kamera érdemi funkció, nem öncélú fotó.

A **Profil megosztása** gomb a játékos azonosító és sportadatainak megosztási lapját nyitja meg (`Share.Default.RequestAsync`). Ez a második, érdemi mobil funkció.

Androidon a kameraengedély a `Platforms/Android/AndroidManifest.xml` fájlban, iOS-en a felhasználónak szóló indoklás a `Platforms/iOS/Info.plist` fájlban szerepel. A tényleges kamerát Android vagy iOS eszközön/emulátoron kell próbálni; Windows alatt a készülék kamerájának támogatása eltérhet.

## Kézi kipróbálás

1. Futtatás után a **Játékosok** fülön hozz létre egy csapatot.
2. Válaszd ki, majd vigyél fel játékost. Ellenőrizd a kötelező mezőket és a csapaton belüli egyedi mezszámot.
3. Válassz ki egy játékost a listában; megnyílik a profiloldal. Add meg a magasságokat, készíts képet, majd mentsd el.
4. Menj vissza, nyisd meg újra a profilt, és ellenőrizd, hogy a kép/adatok megmaradtak.
5. A **Meccsek** fülön próbáld ki az új, módosítás és törlés folyamatát, végül indítsd újra az appot a perzisztencia ellenőrzéséhez.

## Fordítás

```powershell
dotnet build .\VolleyStats\VolleyStats.csproj -f net9.0-windows10.0.19041.0
```

Android teszteléshez telepített MAUI Android workload és Android emulátor vagy USB-hibakeresést engedélyező telefon kell.
