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

### Szoftver architektúra
![Szoftver architektúra diagram](../Docs/imgs/sp/software_architecture_diagram.png)

### Képernyőtervek
#### Kezdőképernyő
![Kezdőképernyő terv](../Docs/imgs/designs/home_page.png)

#### Saját profil
![Saját profil terv](../Docs/imgs/designs/own_profile.png)

#### Menü
![Menü terv](../Docs/imgs/designs/menu.png)

#### Saját, meglévő hirdetések menedzselése
![Menedzselés terv](../Docs/imgs/designs/manage_lodgings.png)
#### Komponensek
a fenti diagramból a komponensek leírása

Komponens neve | Magyarázat |
-------------- | ---------- |
#### Interfacek

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
admin_login | Authentication | |
admin_moderate | Admin supervisement | |
admin_supervise | Admin supervisement | |
bu_browse | User reserving | |
bu_search | User reserving | |
bu_reserve | User reserving | |
bu_cancel | User reserving | |
bu_rate | User reserving | |
au_register	| Authentication | |
au_login | Authentication | |
au_post | Lodging management | |
au_modify | Lodging management | |
au_delete | Lodging management | |
c_register | Authentication | |
c_login | Authentication | |
c_post | Lodging management | |
c_modify | Lodging management | |
c_delete | Lodging management | |

## Biztonság
leírni a titkosítási módszereket, hibakezelési módszereket stb.

## Naplózás és monitorozás
kifejteni a naplózás módját és szintjeit (pl rendszerinformáció, figyelmeztetés, hibaüzenet stb)

## Környezet-függő paraméterek
Paraméter neve | Érték DEV környezetben |
-------------- | ---------------------- |
