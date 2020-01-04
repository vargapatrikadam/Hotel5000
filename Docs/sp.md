# Hotel5000 rendszerterv

## Bevezetés

### Szójegyzék és rövidítések

Rövidítés | Magyarázat | 
--------- | ----------
interface | nem konkrét programozási értelemben vett interface, hanem az applikáció  vagy komponens egy végpontja, elérési módja |

### Architektúra áttekintése
![Magas szintű architektúra diagram](../Docs/imgs/sp/high_level_architecture.png)

## Rendszer architektúra
![Részletes architektúra diagram](../Docs/imgs/sp/detailed_architecture_diagram.png)

### Backend szoftver architektúra
![Backend architektúre diagram](../Docs/imgs/sp/backend_architecture.png)
#### Komponensek
a fenti diagramból a komponensek leírása

Komponens neve | Magyarázat |
-------------- | ---------- |
#### Interfacek
az itt definiált interfaceket a külön interface specifikációban részletezzük

Azonosító | Név | Magyarázat |
--------- | --- | ---------- |

### Frontend szoftver architektúra
frontend részletes diagram ide
#### Komponensek
a fenti diagramból a komponensek leírása

Komponens neve | Magyarázat |
-------------- | ---------- |
#### Interfacek
az itt definiált interfaceket a külön interface specifikációban részletezzük

Azonosító | Név | Magyarázat |
--------- | --- | ---------- |

### Adatbázisréteg felépítése
szöveg ide
#### Adatbázis(ok)
##### Adatbázis_01
Tulajdonságok | Konfiguráció swssdsasda |
------------- | ------------ |
Név |
Technológia |
Collation |
Egyéb |
##### Adatmodell
diagram ide
##### Adatbázis objektumok
Tábla_01

Mező név | Típus/hossz | Kötelező | Érték/validáció | Kulcs | Megj. |
-------- | ----------- | -------- | --------------- | ----- | ----- |
mező 1 | típus 1 | igen | érték validáció | PK | megjegyzés |

Tábla_01 megszorítások

Azonosító | Tábla 1 | Mező 1 | Tábla 2 | Mező 2 | Típus |
--------- | ------- | ------ | ------- | ------ | ----- |

## Követelmények megvalósítása

URS azonosító | Kapcsolódó komponensek azonosítói | Egyéb |
------------- | --------------------------------- | ----- |
admin_login | Admin supervisement | |
admin_moderate | Supervise lodgings | |
admin_supervise | Supervise users | |
bu_browse | Browse lodgings | |
bu_search | Browse lodgings | |
bu_reserve | Reserve lodgings | |
bu_cancel | Cancel lodgings | |
bu_rate | Rate lodgings | |
au_register	| Register basic user | |
au_login | Private individual lodging management | |
au_post | Private individual lodging management | |
au_modify | Private individual lodging management | |
au_delete | Private individual lodging management | |
c_register | Register company | |
c_login | Company lodging management | |
c_post | Company lodging management | |
c_modify | Company lodging management | |
c_delete | Company lodging management | |

## Biztonság
leírni a titkosítási módszereket, hibakezelési módszereket stb.

## Naplózás és monitorozás
kifejteni a naplózás módját és szintjeit (pl rendszerinformáció, figyelmeztetés, hibaüzenet stb)

## Környezet-függő paraméterek
Paraméter neve | Érték DEV környezetben |
-------------- | ---------------------- |
