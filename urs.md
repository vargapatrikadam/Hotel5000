# Hotel5000 követelményspecifikáció

## **Jelenlegi helyzet**
A mai piacon kevés az alternatíva az online szállásfoglalásra. A https://szallas.hu a legismertebb magyar eredetű ilyen rendszer itthon monopolhelyzetet élvez. 

Magánszemélyként nincs lehetőség olyan szálláshirdetésre (saját lakás/szoba) ami nem albérlet, hanem ami egy nyaralás idejéig nyújt szálláshelyet. 

## **Vágyálom rendszer**
A rendszer egyszerűen kezelhető alternatívát képez bárki számára szállásfoglalás szempontjából akár belföldi akár külföldi utakra. A rendszer letisztulsága miatt bármely korosztály számára könnyen átlátható és kezelhető.  

A rendszer könnyen használható lehetőséget biztosítson cégeknek és magánszemélyeknek szálláshirdetsére egyaránt. A cégek és magánszemélyek által hirdetett szállások elkülönítve jelenjenek meg, de a két felület között könnyű legyen a váltás. 

Csak "jóváhagyott" magánszemélyek és cégek tudjanak feladni szálláshirdetést.

## **Jogi háttér**

## **Jelenlegi üzleti folyamatok modellje**
### Magánszemély szállásfoglalása
1. Felkeres egy keresőmotoron egy szállásfoglaló weboldalt
2. Rákeres az úticélra ahová szeretne utatni, vagy az oldalon böngészik a lehetőségek közül
3. Kiválasztja az elérhető dátumok közül az utazás érkezés és távozás időpontját
4. Kiválasztja a szobakiosztást, illetve felnőttek és gyerekek számát
5. Megadja a foglaláshoz szükséges adatokat
6. A megadott e-mail címre küldött visszaigazolást elfogadja a felhasználó
7. A fizetés módjától függően fizet
 
### Magánszemély szálláshirdetése
Jelenleg nem létező opció, vagy nem erre a célre létrehozott platformon történik az efféle szálláshirdetés.

### Cég szálláshirdetése
1. A cég által megbízott alkalmazott(ak) felkeresik az online lehetőségeket a cégük hirdetésére
2. Regisztrálnak a felületen a cég nevében
3. Megadják a felületen a cégük adatait
4. Menedzselik a cégükhöz tartozó hirdetéseket

## **Igényelt üzleti folyamatok modellje**
A magánszemélyek szállásfoglalása és cégek szálláshirdetésének folyamatai megfelelőek, viszont mivel nincsen megfelelő felület a magánszemélyeknek a szálláshirdetésre, ezért a rendszerünkben a már kiforrott előbb említett két üzleti folyamat mellett lehetőséget biztosítunk egy platformon mind a 3 üzleti folyamatra.

### Magánszemély szálláshirdetése
1. A személy hirdetőként regisztrál az alkalmazásba, melynek különböző kritériumai vannak:
   - érvényes alapadatok,
   - telefonszám,
   - személyigazolványszám
2. A személy menedzseli az általa feladott hirdetéseket:
   - új hirdetés feladása esetén:
     - megadja a szállás helyét,
     - foglalható időpontokat,
     - árat
   - már elavult vagy elévült hirdetések törlése/módosítása  