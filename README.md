# 8086 (sy/e)mulator

#

## > projekcior.exe
>wywołuje okno wyboru trybu w jakim chcemy pracować:

## shell 
> &nbsp;&nbsp;wszystkie instrukcje assemblera należy wpisywać pojedynczo.\
> &nbsp;&nbsp;od razu wywołuje obraz pamięci po wykonanej instrukcji.\
> &nbsp;&nbsp;korzystanie ze skoków nie jest dozwolone.\
> &nbsp;&nbsp;komenda **exit** umożliwia wyjście z programu. można tego dokonać również za pomocą wciśnięcia klawiszy ctrl-c 
## >projekcior.exe --shell
>&nbsp;&nbsp;uruchamia tryb shell z pominięciem okna wyboru.
## > file nazwa_pliku [--debug]
> &nbsp;&nbsp;gdzie nazwa_pliku to ścieżka do programu
>> &nbsp;&nbsp;wywołuje natychmiastowe uruchomienie programu i ukazanie jego efektu końcowego.\
>> &nbsp;&nbsp;może zostać tymczasowo zatrzymany poprzez przerwania, a także użycie instrukcji in lub out, które wymagają reakcji użytkownika.
>> 
> &nbsp;&nbsp;dopisanie --debug ustawi flagę TF powodującą przerwanie po każdej instrukcji.
>> &nbsp;&nbsp;umożliwia śledzenie krok po kroku wykonywania programu i obserwację zmian zachodzących w pamięci.\
>> &nbsp;&nbsp;w trybie wczytywania z pliku jest możliwe swobodne korzystanie z instrukcji skoków.
>> 
 &nbsp;&nbsp; możliwa jest również bezpośrednia zmiana trybu shell i file poprzez argumenty uruchamiania programu.
> 
## > projekcior.exe --file nazwa_pliku [--debug]
> &nbsp;&nbsp;uruchamia tryb file z pominięciem okna wyboru

# instrukcje
### 1. przesyłania i modyfikacji danych 
 ##### &nbsp;&nbsp;&nbsp;&nbsp; mov {1}, {2}
 >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;kopiuje wartość argumentu 2 do argumentu 1 

##### &nbsp;&nbsp;&nbsp;&nbsp; push {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;wstawia wartość argumentu 1 na stos

##### &nbsp;&nbsp;&nbsp;&nbsp; pop {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pobiera ostatnią wartość ze stosu i zapisuje ją w argumencie 1

##### &nbsp;&nbsp;&nbsp;&nbsp; xchg {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;podmienia dane przechowywane w argumencie 1 z danymi w argumencie 2

##### &nbsp;&nbsp;&nbsp;&nbsp; lea {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;zapisuje w argumencie 1 adres pamieci podanej w argumencie 2

##### &nbsp;&nbsp;&nbsp;&nbsp; lds {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;zapisuje w argumencie 1 adres pamieci podanej w argumencie 2 i dodaje do tego wartość segmentu DS

##### &nbsp;&nbsp;&nbsp;&nbsp; les {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;zapisuje w argumencie 1 adres pamieci podanej w argumencie 2 i dodaje do tego wartość segmentu ES

##### &nbsp;&nbsp;&nbsp;&nbsp; lahf
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;kopiuje prawe 8 bitów z rejestru flag do rejestru AH

##### &nbsp;&nbsp;&nbsp;&nbsp; sahf
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;kopiuje lewe 8 bitów z rejestru flag do rejestru AH

##### &nbsp;&nbsp;&nbsp;&nbsp; popf
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pobiera ostatnią wartość ze stosu i zapisuje ją w rejestrze flag

##### &nbsp;&nbsp;&nbsp;&nbsp; pushf
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;wstawia wartość rejestru flag na stos

##### &nbsp;&nbsp;&nbsp;&nbsp; stc
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ustawia flagę CF(przeniesienia) na 1

##### &nbsp;&nbsp;&nbsp;&nbsp; clc
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ustawia flagę CF(przeniesienia) na 0


##### &nbsp;&nbsp;&nbsp;&nbsp; in {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;argument 2 oryginalnie miał oznaczać port z którego odczytujemy dane (liczbę albo rejestr DX), ale w tym symulatorze jest ignorowany i wymaga ręcznego wpisania liczby poprzez użytkownika. zostaje ona wprowadzona do argumentu 1

##### &nbsp;&nbsp;&nbsp;&nbsp; out {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;argument 1 oryginalnie miał oznaczać port na którym wprowadzamy dane (liczbę albo rejestr DX)\
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;argument 2 oryginalnie miał być liczbą, jaka miała zostać wypisana (tylko rejestr AX lub AL)\
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;w tym symulatorze argument 1 pozostaje bez zmian, argumetnt 2 zostaje wypisany w konsoli w postaci "wyjście: xxx"

### 2. arytmetyczne

##### &nbsp;&nbsp;&nbsp;&nbsp; add {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dodaje do argumentu 1 argument 2

##### &nbsp;&nbsp;&nbsp;&nbsp; sub {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;odejmuje od argumentu 1 argument 2

##### &nbsp;&nbsp;&nbsp;&nbsp; adc {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dodaje do argumentu 1 argument 2 i jeśli CF jest ustawione dodaje również '1'

##### &nbsp;&nbsp;&nbsp;&nbsp; sbb {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;odejmuje od argumentu 1 argument 2 i jeśli CF jest ustawione odejmuje również '1'

##### &nbsp;&nbsp;&nbsp;&nbsp; inc {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;dodaje '1' do argumentu 1

##### &nbsp;&nbsp;&nbsp;&nbsp; dec {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;odejmuje '1' od argumentu 1

##### &nbsp;&nbsp;&nbsp;&nbsp; aaa
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;poprawia sumę po dodawaniu rozpakowanego BCD
>
##### &nbsp;&nbsp;&nbsp;&nbsp; aas
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;poprawia różnicę po odejmowaniu rozpakowanego BCD

##### &nbsp;&nbsp;&nbsp;&nbsp; daa
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;poprawia sumę po dodawaniu spakowanego BCD

##### &nbsp;&nbsp;&nbsp;&nbsp; das
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;poprawia różnicę po odejmowaniu spakowanego BCD
>
##### &nbsp;&nbsp;&nbsp;&nbsp; mul {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;jeśli w argumencie 1 jest coś 8 bitowego - AL jest mnożone przez argument 1, a wynik tego działania jest zapisywany w AX\
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;jeśli w argumencie 1 jest coś 16 bitowego - AX jest mnożone przez argument 1, wynik jest zapisywany w DX i AX, gdzie górna połowa wyniku jest w DX, a dolna połowa w AX 

##### &nbsp;&nbsp;&nbsp;&nbsp; imul {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; mul ze znakiem

##### &nbsp;&nbsp;&nbsp;&nbsp; div {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;jeśli w argumencie 1 jest coś 8 bitowego - AX jest dzielone przez argument 1, wynik tego działania jest zapisywany w AH, a reszta z dzielenia w AL\
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;jeśli w argumencie 1 jest coś 16 bitowego - DX:AX jest dzielone przez argument 1, wynik tego działania jest zapisywany w AX, a reszta z dzielenia w DX



##### &nbsp;&nbsp;&nbsp;&nbsp; idiv {1} 
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; div ze znakiem

##### &nbsp;&nbsp;&nbsp;&nbsp; aad
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; przygotowuje AX przed dzieleniem BCD

##### &nbsp;&nbsp;&nbsp;&nbsp; aam
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; przygotowuje AX po mnożeniu BCD

##### &nbsp;&nbsp;&nbsp;&nbsp; cbw
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; przepisuje znak z AL na AH (jeśli AL jest ujemne, to wypełnia AH jedynkami - w przeciwnym wypadku - zerami)

##### &nbsp;&nbsp;&nbsp;&nbsp; cwd
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; rozszerza znak z AX na DX (jeśli AX < 0 => DX wypełnia jedynkami, a gdy AX >= 0 => DX wypełnia zerami)

##### &nbsp;&nbsp;&nbsp;&nbsp; neg {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; zmienia znak argumentu 1

##### &nbsp;&nbsp;&nbsp;&nbsp; cmp {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; porównuje argumenty

### 3. logiczne

##### &nbsp;&nbsp;&nbsp;&nbsp; not {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; odwraca bity

##### &nbsp;&nbsp;&nbsp;&nbsp; shl {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; wykonuje logiczne przesunięcie w lewo argumentu 1 o tyle miejsc ile określa argument 2

##### &nbsp;&nbsp;&nbsp;&nbsp; sal {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; arytmetyczne shl

##### &nbsp;&nbsp;&nbsp;&nbsp; shr {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; wykonuje logiczne przesunięcie w prawo argumentu 1 o tyle miejsc ile określa argument 2

##### &nbsp;&nbsp;&nbsp;&nbsp; sar {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; arytmetyczne shr

##### &nbsp;&nbsp;&nbsp;&nbsp; rol {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; przesuwa skrajny lewy bit za skrajny prawy tyle razy, ile zapisano w argumencie 2 

##### &nbsp;&nbsp;&nbsp;&nbsp; ror {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; przesuwa skrajny prawy bit za skrajny lewy tyle razy, ile zapisano w argumencie 2 

##### &nbsp;&nbsp;&nbsp;&nbsp; rcl {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; rol z uwzględnieniem CF

##### &nbsp;&nbsp;&nbsp;&nbsp; rcr {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ror z uwzględnieniem CF

##### &nbsp;&nbsp;&nbsp;&nbsp; and {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; logiczne AND na argumentach

##### &nbsp;&nbsp;&nbsp;&nbsp; or {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; logiczne OR na argumentach

##### &nbsp;&nbsp;&nbsp;&nbsp; xor {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; logiczne XOR na argumentach

##### &nbsp;&nbsp;&nbsp;&nbsp; test {1}, {2}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; and tylko bez zapisywania wyniku w arg 1

### 4. skoków

##### &nbsp;&nbsp;&nbsp;&nbsp; jmp {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok bezwarunkowy

##### &nbsp;&nbsp;&nbsp;&nbsp; call {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; niezaimplementowne

##### &nbsp;&nbsp;&nbsp;&nbsp; ret
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; niezaimplementowne

##### &nbsp;&nbsp;&nbsp;&nbsp; je
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczby są równe

##### &nbsp;&nbsp;&nbsp;&nbsp; jz
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy wynik = 0

##### &nbsp;&nbsp;&nbsp;&nbsp; jne
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczby są nierówne


##### &nbsp;&nbsp;&nbsp;&nbsp; jnz
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy wynik != 0

##### &nbsp;&nbsp;&nbsp;&nbsp; jo
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy wystąpiło przepełnienie

##### &nbsp;&nbsp;&nbsp;&nbsp; jno
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy nie wystąpiło przepełnienie

##### &nbsp;&nbsp;&nbsp;&nbsp; jc
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy wystąpiło przeniesienie

##### &nbsp;&nbsp;&nbsp;&nbsp; jnc
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy nie wystąpiło przeniesienie

##### &nbsp;&nbsp;&nbsp;&nbsp; jp
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy parzysta liczba bitów ustawionych na 1

##### &nbsp;&nbsp;&nbsp;&nbsp; jpe
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy parzysta liczba bitów ustawionych na 1

##### &nbsp;&nbsp;&nbsp;&nbsp; js
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy znak wyniku jest ujemny

##### &nbsp;&nbsp;&nbsp;&nbsp; jns
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy znak wyniku jest nieujemny

#### po porównaniu bez znaku:

>##### &nbsp;&nbsp;&nbsp;&nbsp; jb
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest mniejsza
>##### &nbsp;&nbsp;&nbsp;&nbsp; jnae
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba nie jest ani większa, ani równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jbe
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest mniejsza lub równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jna
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest niewiększa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jae
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest większa lub równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jnb
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest niemniejsza
>##### &nbsp;&nbsp;&nbsp;&nbsp; ja
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest większa 
>##### &nbsp;&nbsp;&nbsp;&nbsp; jnbe
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba nie jest ani mniejsza, ani równa

#### po porównaniu ze znakiem:

>##### &nbsp;&nbsp;&nbsp;&nbsp; jl
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest mniejsza
>##### &nbsp;&nbsp;&nbsp;&nbsp; jnge
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba nie jest ani większa, ani równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jle
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest mniejsza lub równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jng
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest niewiększa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jge
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest większa lub równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jnl
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest niemniejsza
>##### &nbsp;&nbsp;&nbsp;&nbsp; jnle
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba nie jest ani mniejsza, ani równa
>##### &nbsp;&nbsp;&nbsp;&nbsp; jg
>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy liczba jest większa 

##### &nbsp;&nbsp;&nbsp;&nbsp; jcxz
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; skok, gdy rejestr CX = 0

### 5. przerwań

##### &nbsp;&nbsp;&nbsp;&nbsp; int {1}
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; wywołuje przerwanie z kodem określonym w arg 1

##### &nbsp;&nbsp;&nbsp;&nbsp; into
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; wywołuje przerwanie o kodzie 4 jeśli flaga OF = 1

### 5. pozostałe

##### &nbsp;&nbsp;&nbsp;&nbsp; exit
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; działa tylko i wyłącznie w trybie konsolowym\
>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; powoduje zakończenie programu












w połownie tworzenia tego dzieła zarezerwowałam sobie miejsce na cmentarzu. 
