# Hra 4 Life

### Autoři
Programování: Lukáš Caha

Design: Daniel Knotek, Robert Poláček

### Pro předmět
NPRG031 - Programování II

Matematicko-fyzikální fakulta Univerzity Karlovy, Softwarové a datové inženýrství

## Anotace

Hra 4 Life pro webové prohlížeče je založena na komplexních schopnostech hráčské postavy a následného návrhu úrovní. Popíšu propojení mezi jednotlivými objekty, ze kterých se skládá hráčská postava. Avšak ostatním mechanikám, které ve hře jsou jako je například ovládání hlasitosti, nebo pohyb kamery nebudu věnovat moc pozornosti.

## Cíl hry

Cílem hry je sesbírat všechny artefakty, které se vyskytují na konci každé úrovně.

## Herní mechaniky

Hlavní překážkou v cestě na konec úrovně jsou nepřátele a prostředí, kterým musí hráč projít. To ústí ve dvě základní herní mechaniky, které ve spojení tvoří 4 Life.

1. Bojový systém
2. Interakce a pohyb v prostředí

### Bojový systém

Samotný bojový systém se skládá z:

* Nepřátel - autonomní entity, jejichž úkolem je přiblížit se co nejvíce hráči a zároveň vystřelit co nejvíce střel na jeho poškození; po jejich smrti upustí jeden z vylepšujících boxů, který může hráč sebrat
* Střílení - hráč má k dispozici tři rozdílné zbraně, může namířit na libovolné místo a vystřelit projektil, ty následně interagují s prostředím explozí; exploze ovlivňuje i interaktivní objekty v prostředí
* Životů - hráč má omezený počet životů, které ztrácí při zásahu od nepřátel; počet je indikován štítem okolo hráče; životy i maximální počet může hráč zvýšit pomocí vylepšujících boxů

### Interakce a pohyb v prostředí

Prostředí tvoří druhou, více myšlenkově náročnou, část postupu úrovní. Obdobně jako bojový systém je tato část tvořena více jednoduchými mechanikami, které v kombinaci umožňují velmi rozmanitý pohyb.

* Pohyb do stran - hráč se v libovolné pozici (na zemi i ve vzduchu) může pohybovat konstantní rychlostí do obou stran; postava se dívá ve směru v koordinaci s mířením zbraně, je tedy možné jít pozadu a mířit na opačnou stranu
* Skákání - skok může hráč uskutečnit pouze, pokud se nachází na zemi nebo pokud je přichycen hákem ke zdi nebo stropu
* Záchytný hák - umožní hráči vystřelit hák libovolným směrem a pokud se zachytí o zeď, přitáhne hráče k místu zachycení
* Posouvání objektů - pohyblivé objekty v prostředí se dají posouvat, když k nim hráč přijde a chůzí zatlačí
* Odhazování objektů explozí - při každé explozi projektilu jsou okolní objekty odhozeny patřičnou silou

## Seznam skriptů

### Hráčská postava a propojení skriptů
* Artefact_collector.cs - Drží seznam všech existujících artefaktů. Při sebrání artefaktu ukončí úroveň a označí ho za sebraný. V hlavním menu zobrazuje sebrané artefakty.
* Combat.cs - V každém snímku míří aktuální zbraní na kurzor myši. Při zmáčknutí střelby vystřelí projektil. Zároveň uchovává statistiky zbraní.
* Hook.cs - Vystřeluje hák směrem na kurzor myši. Spravuje všech 5 fází.
  * still - Hráč má  hák u sebe.
  * expanding - Hák se právě natahuje směrem na myš.
  * rollingBack - Po neúspěšném uchycení se hák vrací zpět k hráči.
  * pullingUp - Přitahuje hráče k místu uchycení.
  * hold - Drží hráče na místě uchycení.
* Change_guns.cs - Mění mezi třemi různými zbraněmi.
* Movement.cs - Skript řeší pohyb do stran, skákání a otáčení postavy do směru pohybu.
* Pickup.cs - Sbírá objekty na zemi a posílá zprávu o sebrání náležitým skriptům.
* Shield.cs - Spravuje hráčovy životy a vizuální stránku štítu.
### Projektily a nepřátelé
* Attack.cs - Na nepříteli. Pokud je hráč v určité vzdálenosti, tak se vydá jeho směrem a začne střílet.
* Bullet_trajectory.cs - Na projektilu. Při detekci kolize exploduje, objektu udělí poškození a odhodí okolní objekty.
* Enemy_stats.cs - Na nepříteli. Udržuje životy nepřítele a při poklesu pod 0 zničí objekt nepřítele.
### Ostatní
* Artefact.cs - Na artefaktu. Při kontaktu s hráčem dá skriptu Artefact_collector.cs na hráči vědět jak se tento sebraný artefakt jmenuje.
* FollowTarget.cs - Na kameře. Sleduje v úrovních hráče a při explozi se zatřese pro efekt.
* Options.cs - Ovládání hlasitosti hudby a zvukových efektů.
* Pickupable.cs - Na vylepšujících boxech. Dává skriptu Pickup.cs vědět o typu objektu, který byl sebrán.
* SlideMenu.cs - Tabulátorem vysouvá statistiky o hráčovi.
* Spawn_player.cs - Na objektu, který určuje začáteční pozici hráče. Posune hráče na svou pozici a vynuluje všechny síly v komponentu RigidBody2D, aby hráč neodletěl pryč. Rovněž inicializuje několik proměnných potřebných pro funkci ostatních skriptů, které se nacházejí na úrovni, nikoli na hráči.
* Teleporter.cs - Mění scény.

## Přesun mezi scénami

Jelikož se reference objektů ruší, nebo nikdy nejsou inicializovány, pokud není referencovaný objekt ve stejné scéně, je nutné propojovat objekty za běhu. Největší problém činí hráčská postava, která cestuje mezi scénami a na sobě si zachovává všechny data o postupu a statistky. Proto si při každém načtení scény hráče najde skript Spawn_player.cs a distribuuje jeho referenci mezi ostatní skripty, které se nachází ve scéně, jako například nepřátelé nebo artefakt.

Skripty, které jsou na hráčovi nebo jeho dceřiných objektech jsou mezi sebou neustále propojeny. Proto jsem se rozhodl velkou část uživatelského rozhraní umístit právě pod hráče. Umožňuje to také existenci pouze jednoho objektu v celé hře s danou funkcí. Příkladem tohoto je nastavení hlasitosti, které by jinak muselo být vložené v každé scéně separátně, nebo objekt EventSystem, který musí být v každé scéně právě jednou, aby fungovali elementy uživatelského rozhraní.

Kódově je zachování hráče mezi scénami (skript Artefact_collector.cs, který spravuje uchování herních dat o artefaktech a podobně) řešeno pomocí funkce, která zachová objekt při načtení nové scény:

```c#
DontDestroyOnLoad(gameObject);
```

Ale po opětovném navrácení do scény kde se hráč nachází původně vznikají kopie. A to nesmí rozhodně nastat. Proto vytvořím veřejnou statickou instanci tohoto skriptu:

```c#
public static Artefact_collector instance = null;
//spuštěno při prvotním objevení objektu, proto se nezničí již používaný hráčský objekt
void Start(){						
    if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);		//odstraní duplicitního hráče
        }
}

```

## Struktura hráčského objektu

* Pepe - Hlavní objekt, drží texturu, fyzikální komponent, kolizní box, animace chůze a stání, Movement.cs, Shield.cs, Artefact_collector.cs, Pickup.cs, Change_guns.cs, zvuky pádu, skoku a zásahu
  * "Top left" a "Bottom right" - Objekty jejichž pozice označuje obdélník v jehož oblasti musí být skákatelný objekt, jinak hráč nemůže vyskočit.
  * Teleporter - Tato instance se používá pro změnu scény po sebrání artefaktu. Drží totiž Teleporter.cs.
  * Pistol, Rifle a Shotgun - Drží texturu zbraně a skript Combat.cs a vlastnostmi této zbraně. Také obsahuje audio výstřelu.
    * "Barrel end" - Místo kde se objeví po výstřelu projektil.
    * "Muzzle flash" - Vizuální efekt spuštěný po výstřelu.
  * Shield - Vizuální vzhled štítu, skript Shield.cs na objektu Pepe mění různé fáze zničení štítu.
  * Hook - Skript Hook.cs a komponent na vykreslování lana háku. Také obsahuje fyzikální komponent, aby mohl skript registrovat kolize.
    * "Hook head" - Objekt, který je vystřelován a obsahuje kolizní oblast pro registraci záchytu o zdi.
  * Canvas - Obsahuje v sobě veškeré uživatelské rozhraní. A obsahuje skript na vysouvací statistiky SlideMenu.cs.
    * "Level completed" - Obrazovka, která se zobrazí po sebrání artefaktu.
    * #Menu - Vysouvací statistiky. (# znamená, že objekt je vyhledáván pomocí názvu)
    * Gear - Vysouvací nastavení zvuku. Obsahuje skript Options.cs, vysouvání je zařízeno pomocí animací.
  * Soundtrack - Obsahuje herní hudbu.
  * EventSystem - Registruje vstupy v podobě kliků na tlačítka a podobné.

## Ovládání

Na ovládání hry je potřeba klávesnice a myš.

### Pohyb

* W, šipka nahoru, Space - skok
* A, šipka doleva - pohyb doleva
* D, šipka doprava - pohyb doprava
* E - vypuštění háku
* Esc - ukončení háku 

 ### Bojový systém

* levé tlačítko myši - střelba
* kolečko myši, alfanumerické číslice - změna zbraní

## Osobní zhodnocení

Při programování jsem používal program pro sledování času, abych do budoucna věděl, jak časově náročný je vývoj podobné hry. Z toho vyplývá několik statistik, které jsou plné zkušeností.

### Časová náročnost

Podíl celkového času po odečtení testovacích buildů pro externí zhodnocení.

* Postava hráče - 42% času
* Prostředí - 34% času
* Nepřátelé - 15% času
* Oprava chyb - 9% času

### Průběh práce

1. Chůze hráče, skákání, animace hráče, jednoduchá kamera, střílení, odhazování objektů, základní úroveň pro testování
2. Animace, chůze a poškození nepříteli
3. Hlavní menu v podobě tlačítka "Jdi do úrovně"
4. Dobíjení životů po zabití nepřítele, základní statistiky vlevo na obrazovce (životy, vzdálenost od artefaktu, čas)
5. Artefakt
6. Rotace zbraně, zvukové a vizuální efekty výstřelu
7. Vizuální reprezentace štítu
8. Větší mapa a nepřátele na testování
9. Nepřítel dává poškození hráči
10. Druhá úroveň, přenášení mezi úrovněmi, po sebrání artefaktu výhra úrovně, lepší základna, možnost návratu do základny v půlce úrovně, zobrazení sebraných artefaktů, animace dokončení úrovně
11. Statistiky zbraní, vylepšovací boxy
12. Hudba
13. Více zbraní, každá s vlastními efekty, zvuky poškození postav, alternativní způsoby změny zbraní
14. Zvukové efekty skákání a dopadu
15. Vizuální pruh životů pro hráče i nepřítele
16. Opravy poměru stran, padání ze světa, alternativní skákání, mizení statistik, nestřílení při zmáčknutí tlačítka, odstranění skákání po stropech, aktualizace statistik zbraní
17. Vysunovací statistiky pomocí TAB, nastavení hudby, ukládání zvukového nastavení po vypnutí
18. Padající boxy s vylepšením
19. Přichytávání hákem
20. Průběžné testovací buildy

### Plány do budoucna

* Více a lepší zvukové a vizuální efekty
* Vlastní grafika prostředí, postav i objektů
* Návrat do základny zpět přes úroveň
* Přehlednější základna
* Herní měna
* Propracovanější systém vylepšení
* Více druhů nepřátel
* Ukládání hry
* Obtížnější průchod
* Omezené náboje
* Návod na hraní

### Subjektivní zhodnocení výběru tématu

Programování nediskrétní hry pro účely školního zadání je vždy složitější než ostatní možná témata, protože neumožňuje odhalit drobné chyby v programu pomocí logického myšlení. Místo toho je nutné strávit velký čas na testování. Na druhou stranu jsem rád, že jsem si vybral více vizuální téma, jelikož jsem měl možnost testovat funkčnosti po krátkých časových intervalech, namísto složitých algoritmů, kde musí být celý algoritmus hotový a odladěný, než ukáže reálné výsledky.

Také jsem rád za výběr frameworku, protože podobné funkce můžou být vytvořeny několika způsoby. Jako příklad uvedu obě vysouvací menu. Statistiky zleva pomocí klávesy TAB a ovládání hlasitosti zprava pomocí kliknutí na ozubené kolečko. Levý panel se vysouvá pomocí skriptu, který spouští animaci a pravý panel je vytvořen čistě pomocí animací tlačítka. Určitě by šel vytvořit univerzální skript na vysouvání obou panelů zároveň, ale možnost výběru se vždy hodí.

Tím bych navázal na znovupoužitelnost mého kódu. Většina z mých skriptů jsou účelné přesně pro jejich funkci, ale nachází se tam i pár skriptů, které jsou použité ve více různých situacích:

* Skript na kameru, který obsahuje třesení při explozi je použit jednak na kameru v základně, kde se kamera nepohybuje a jednak na kameru v úrovních, kde následuje hráče.
* Skript na hráčovu zbraň je stejný na všech třech zbraních, jediné věci co se liší jsou statistiky, zvuky a efekty. Ale všechno je změněno přes Inspector v Unity.
* Teleportér hraje při vstupu do úrovně zvuk teleportace, ale jinak ne. Je použit stejný skript na všechny teleportéry v celé hře.
* Skript generující padající boxy z nepřátel umožňuje z jednoho objektu díky výměně textury udělat 15 různých objektů (i víc při dostatku textur).
* Skripty na nepřítele by po úpravě mohli být stejně univerzální na více typů nepřátel, jako skripty na zbraně.