const API_KEY = 'AIzaSyAGfYUfpBp2kZkQKVw_hq-nHq1q1NhJTwo';
const data = [
    {name: 'Giżycko  Miłki', range: 100},
    {name: 'Białogard  Sławoborze', range: 50},
    {name: 'Opole  Chrzelice', range: 100},
    {name: 'Rabka  Luboń Wielki', range: 20},
    {name: 'Koszalin  Gołogóra', range: 100},
    {name: 'Kraków  Chorągwica', range: 100},
    {name: 'Szczawnica  g. Przehyba', range: 20},
    {name: 'Kłodzko  Czarna Góra', range: 50},
    {name: 'Żagań  Wichów', range: 50},
    {name: 'Wisła  Skrzyczne', range: 100},
    {name: 'Rzeszów  Sucha Góra', range: 100},
    {name: 'Zakopane  Gubałówka', range: 20},
    {name: 'Katowice  Kosztowy', range: 50},
    {name: 'Zamość  Tarnawatka', range: 50},
    {name: 'Solina  g. Jawor', range: 20},
    {name: 'Ciechanów  ul. Monte Cassino', range: 20},
    {name: 'Kalisz  Mikstat', range: 100},
    {name: 'Konin  Żółwieniec', range: 100},
    {name: 'Wrocław  Ślęża', range: 100},
    {name: 'Olsztyn  Pieczewo', range: 100},
    {name: 'Lublin  Piaski', range: 100},
    {name: 'Ostrołęka  Ławy', range: 60},
    {name: 'Piła  Rusinowo', range: 100},
    {name: 'Jelenia Góra  Śnieżne Kotły', range: 100},
    {name: 'Tarnobrzeg  Machów', range: 20},
    {name: 'Różan', range: 20},
    {name: 'Gorlice  Maślana Góra', range: 20},
    {name: 'Warszawa  Raszyn', range: 100},
    {name: 'Gdańsk  Chwaszczyno', range: 100},
    {name: 'Iława  Kisielice', range: 100},
    {name: 'Przysucha  Kozłowiec', range: 50},
    {name: 'Warszawa  PKiN', range: 20},
    {name: 'Łomża  Szosa Zambrowska', range: 20},
    {name: 'Chruszczewka', range: 20},
    {name: 'Świnoujście  ul. Chrobrego', range: 20},
    {name: 'Szczecin  Kołowo', range: 100},
    {name: 'Lębork  Skórowo Nowe', range: 20},
    {name: 'Suwałki  Krzemianucha', range: 20},
    {name: 'Tarnów  g. Św. Marcina', range: 50},
    {name: 'Leżajsk  Giedlarowa', range: 100},
    {name: 'Bydgoszcz  Trzeciewiec', range: 100},
    {name: 'Kielce  Święty Krzyż', range: 100},
    {name: 'Przemyśl  Tatarska Góra', range: 20},
    {name: 'Siedlce  Łosice', range: 20},
    {name: 'Dęblin  Ryki', range: 20},
    {name: 'Łódź  EC', range: 100},
    {name: 'Zielona Góra  Jemiołów', range: 80},
    {name: 'Elbląg  Jagodnik', range: 20},
    {name: 'Częstochowa  Wręczyca Wielka', range: 100},
    {name: 'Białystok  Krynice', range: 58},
    {name: 'Płock  Rachocin', range: 100},
    {name: 'Poznań  Śrem', range: 100},
    {name: 'Gniezno  Chojna', range: 20},
    {name: 'Zielona Góra  Jemiołów', range: 79},
    {name: 'Jelenia Góra  Śnieżne Kotły', range: 100},
    {name: 'Kłodzko  Czarna Góra', range: 50},
    {name: 'Konin  Żółwieniec', range: 100},
    {name: 'Przemyśl  Tatarska Góra', range: 20},
    {name: 'Gorlice  Maślana Góra', range: 20},
    {name: 'Warszawa  PKiN', range: 20},
    {name: 'Koszalin  Gołogóra', range: 100},
    {name: 'Lębork  Skórowo Nowe', range: 20},
    {name: 'Elbląg  Jagodnik', range: 20},
    {name: 'Suwałki  Krzemianucha', range: 20},
    {name: 'Dęblin  Ryki', range: 20},
    {name: 'Opole  Chrzelice', range: 100},
    {name: 'Szczawnica  g. Przehyba', range: 20},
    {name: 'Tarnów  g. Św. Marcina', range: 50},
    {name: 'Zakopane  Gubałówka', range: 20},
    {name: 'Solina  g. Jawor', range: 20},
    {name: 'Gniezno  Chojna', range: 20},
    {name: 'Warszawa  Raszyn', range: 100},
    {name: 'Piła  Rusinowo', range: 100},
    {name: 'Kalisz  Mikstat', range: 100},
    {name: 'Bydgoszcz  Trzeciewiec', range: 100},
    {name: 'Poznań  Śrem', range: 100},
    {name: 'Wisła  Skrzyczne', range: 100},
    {name: 'Świnoujście  ul. Chrobrego', range: 20},
    {name: 'Białogard  Sławoborze', range: 50},
    {name: 'Białystok  Krynice', range: 67},
    {name: 'Giżycko  Miłki', range: 100},
    {name: 'Ostrołęka  Ławy', range: 60},
    {name: 'Tarnobrzeg  Machów', range: 20},
    {name: 'Żagań  Wichów', range: 50},
    {name: 'Rzeszów  Sucha Góra', range: 100},
    {name: 'Siedlce  Łosice', range: 50},
    {name: 'Wrocław  Ślęża', range: 100},
    {name: 'Zamość  Tarnawatka', range: 50},
    {name: 'Iława  Kisielice', range: 100},
    {name: 'Olsztyn  Pieczewo', range: 100},
    {name: 'Kielce  Święty Krzyż', range: 100},
    {name: 'Lublin  Piaski', range: 100},
    {name: 'Przysucha  Kozłowiec', range: 50},
    {name: 'Różan', range: 20},
    {name: 'Ciechanów  ul. Monte Cassino', range: 20},
    {name: 'Łomża  Szosa Zambrowska', range: 20},
    {name: 'Gdańsk  Chwaszczyno', range: 100},
    {name: 'Szczecin  Kołowo', range: 100},
    {name: 'Płock  Rachocin', range: 100},
    {name: 'Częstochowa  Wręczyca Wielka', range: 100},
    {name: 'Kraków  Chorągwica', range: 100},
    {name: 'Rabka  Luboń Wielki', range: 20},
    {name: 'Katowice  Kosztowy', range: 63},
    {name: 'Leżajsk  Giedlarowa', range: 100},
    {name: 'Łódź  EC', range: 100},
    {name: 'Cieszanów', range: 20},
    {name: 'Dęblin  Ryki', range: 20},
    {name: 'Zielona Góra  ul. Ptasia', range: 20},
    {name: 'Olsztyn  Pieczewo', range: 100},
    {name: 'Piła  Rusinowo', range: 25},
    {name: 'Siedlce  Łosice', range: 40},
    {name: 'Słupiec  g. Kościelec', range: 20},
    {name: 'Opole  Chrzelice', range: 100},
    {name: 'Gdynia  Oksywie', range: 20},
    {name: 'Szczyrk  ul. Poziomkowa', range: 20},
    {name: 'Gdańsk  Wejherowo', range: 20},
    {name: 'Jeleśnia  Góra Krzyżowa', range: 20},
    {name: 'Rajcza  g. Hutyrów', range: 20},
    {name: 'Stryszawa  g. Wojewódka', range: 20},
    {name: 'Strzyżów  Działy', range: 20},
    {name: 'Tarnawa  g. Makówka', range: 20},
    {name: 'Wołkowyja  g. Czaków', range: 20},
    {name: 'Posada Jaśliska', range: 20},
    {name: 'Częstochowa  Wręczyca Wielka', range: 80},
    {name: 'Krynica  g. Parkowa', range: 20},
    {name: 'Międzybrodzie  g. Żar', range: 20},
    {name: 'Poręba Wielka  wzn. Nowa Wieś', range: 20},
    {name: 'Bydgoszcz', range: 20},
    {name: 'Racławice  Wieś', range: 20},
    {name: 'Zawoja  g. Kolisty Groń', range: 20},
    {name: 'Grudziądz  ul. Kalinkowa', range: 20},
    {name: 'Łobez  ul. Podgórna', range: 20},
    {name: 'Starachowice  ul. Martenowska', range: 20},
    {name: 'Turośl', range: 20},
    {name: 'Kraśnik  ul. Lubelska', range: 20},
    {name: 'Mrągowo  ul. Spacerowa', range: 20},
    {name: 'Ostrołęka  Ławy', range: 60},
    {name: 'Katowice  Kosztowy', range: 100},
    {name: 'Wleń  Modrzewie', range: 20},
    {name: 'Konin  Żółwieniec', range: 40},
    {name: 'Jelenia Góra  Śnieżne Kotły', range: 100},
    {name: 'Szyndzielnia', range: 20},
    {name: 'Cisna  g. Potoczyszcze', range: 20},
    {name: 'Gdańsk  Jaśkowa Kopa', range: 20},
    {name: 'Koniaków  g. Ochodzita', range: 20},
    {name: 'Zabrze', range: 20},
    {name: 'Żywiec  g. Grojec', range: 20},
    {name: 'Płock  Radziwie', range: 20},
    {name: 'Tylawa  g. Dział', range: 20},
    {name: 'Świeradów Zdrój  g. Zajęcznik', range: 20},
    {name: 'Muszyna  g. Malnik', range: 20},
    {name: 'Lądek Zdrój  g. Dzielec', range: 20},
    {name: 'Nowy Sącz  Chruślice', range: 20},
    {name: 'Szczytna  g. Szczytnik', range: 20},
    {name: 'Olesno  ul. Leśna', range: 20},
    {name: 'Sucha Beskidzka  g. Sumerówka', range: 20},
    {name: 'Tarnów  g. Św. Marcina', range: 50},
    {name: 'Tylicz  g. Horb', range: 20},
    {name: 'Dobra  g. Nad Kiwajami', range: 20},
    {name: 'Piwniczna  g. Kicarz', range: 20},
    {name: 'Koszalin  Gołogóra', range: 100},
    {name: 'Białogard  Sławoborze', range: 50},
    {name: 'Lębork  Skórowo Nowe', range: 20},
    {name: 'Radom  ul. Przytycka', range: 50},
    {name: 'Giżycko  Miłki', range: 90},
    {name: 'Radków  g. Guzowata', range: 20},
    {name: 'Piechowice  Górzyniec', range: 20},
    {name: 'Kudowa  Góra Parkowa', range: 20},
    {name: 'Konin  Żółwieniec', range: 20},
    {name: 'Iława  Kisielice', range: 50},
    {name: 'Koszalin  Gołogóra', range: 71},
    {name: 'Warszawa  PKiN', range: 20},
    {name: 'Tarnobrzeg  Machów', range: 30},
    {name: 'Wolbrom  Chełm', range: 20},
    {name: 'Elbląg  Jagodnik', range: 20},
    {name: 'Rzeszów  Baranówka', range: 20},
    {name: 'Ścięgny  g. Pohulanka', range: 20},
    {name: 'Poznań  Śrem', range: 20},
    {name: 'Kraków  Chorągwica', range: 100},
    {name: 'Lublin  ul. Raabego', range: 20},
    {name: 'Cieszyn  ul. Mickiewicza', range: 20},
    {name: 'Iwonicz Zdrój  Excelsior', range: 20},
    {name: 'Trójca  g. Jaworów', range: 20},
    {name: 'Ujsoły  g. Kubiesówka', range: 20},
    {name: 'Teodorówka  Dukla', range: 20},
    {name: 'Grybów  g. Kamienna', range: 20},
    {name: 'Ochotnica Górna  g. Utocze', range: 20},
    {name: 'Stryszów  Góra Stryszów', range: 20},
    {name: 'Hoczew  g. Czekaj', range: 20},
    {name: 'Grzywacz', range: 20},
    {name: 'Koszarawa  wzg. Mendralowe', range: 20},
    {name: 'Makarki', range: 20},
    {name: 'Rzepedź  g. Sokoliska', range: 20},
    {name: 'Busko Zdrój', range: 20},
    {name: 'Szczyrk  DW Centrum', range: 20},
    {name: 'Ostrów Mazowiecka  Podborze', range: 20},
    {name: 'Wisła  os. Kozińce', range: 20},
    {name: 'Jedlina  g. Kawiniec', range: 20},
    {name: 'Zahoczewie  Szerokie', range: 20},
    {name: 'Walim  g. Ostra', range: 20},
    {name: 'Gromnik', range: 20},
    {name: 'Świerzawa  ul. Mickiewicza', range: 20},
    {name: 'Krynica  g. Jaworzyna', range: 20},
    {name: 'Żegiestów Zdrój  g. Kiczera', range: 20},
    {name: 'Szczecin  Warszewo', range: 20},
    {name: 'Wrocław  Ślęża', range: 100},
    {name: 'Łanięta', range: 20},
    {name: 'Bydgoszcz  Trzeciewiec', range: 100},
    {name: 'Kalisz  Mikstat', range: 35},
    {name: 'Kraków  Chorągwica', range: 35},
    {name: 'Wągrowiec  ul. Mickiewicza 13a', range: 20},
    {name: 'Kielce  Święty Krzyż', range: 150},
    {name: 'Kielce  EC', range: 20},
    {name: 'Dęblin  Ryki', range: 20},
    {name: 'Rewal  Lędzin', range: 20},
    {name: 'Przysucha  Kozłowiec', range: 20},
    {name: 'Gniezno  Chojna', range: 23},
    {name: 'Kulin  g. Grodziec', range: 20},
    {name: 'Lubawka  g. Święta', range: 20},
    {name: 'Działoszyn  Centrum METEO', range: 20},
    {name: 'Lubawka  Ulanowice', range: 20},
    {name: 'Zielona Góra  Jemiołów', range: 100},
    {name: 'Damasławek', range: 20},
    {name: 'Katowice  Bytków', range: 20},
    {name: 'Piła  Rusinowo', range: 100},
    {name: 'Iława  Kisielice', range: 20},
    {name: 'Gdańsk  Chwaszczyno', range: 100},
    {name: 'Iława  Kisielice', range: 35},
    {name: 'Łanięta', range: 20},
    {name: 'Warszawa  Raszyn', range: 130},
    {name: 'Bircza  g. Kamienna', range: 20},
    {name: 'Olszanica  g. Kiczera', range: 20},
    {name: 'Zatwarnica  g. Wierszek', range: 20},
    {name: 'Kamionka Wlk.  g. Dybówka', range: 20},
    {name: 'Krościenko  g. Stajkowa', range: 20},
    {name: 'Żegiestów Wieś  g. Cypel', range: 20},
    {name: 'Limanowa  g. Lipowe', range: 20},
    {name: 'Rytro  g. Cycówka', range: 20},
    {name: 'Czarna', range: 20},
    {name: 'Kamieńsk  Zwałowisko', range: 20},
    {name: 'Kalnica  g. Wideta', range: 20},
    {name: 'Pruchnik  wzn. Na Zadach', range: 20},
    {name: 'Lutowiska', range: 20},
    {name: 'Zamość  Tarnawatka', range: 50},
    {name: 'Sanok  g. Parkowa', range: 20},
    {name: 'Ustrzyki Dolne  g. Gromadzyń', range: 20},
    {name: 'Węgierska Górka  g. Przybędza', range: 20},
    {name: 'Kętrzyn  ul. Łokietka', range: 20},
    {name: 'Wisła  Skrzyczne', range: 60},
    {name: 'Duszniki Zdrój  Podgórze', range: 20},
    {name: 'Baligród  g. Kiczera', range: 20},
    {name: 'Głuszyca  Kościół', range: 20},
    {name: 'Jabłonka  g. Oskwarkowa', range: 20},
    {name: 'Karpacz Górny  ul. Spokojna', range: 20},
    {name: 'Łącko  g. Cebulówka', range: 20},
    {name: 'Sokołowsko  Polana', range: 20},
    {name: 'Niedzica  g. Biała', range: 20},
    {name: 'Kamienna Góra  g. Kościelna', range: 20},
    {name: 'Zakopane  Gubałówka', range: 20},
    {name: 'Wojcieszów  g. Miłek', range: 20},
    {name: 'Kraków  Krzemionki', range: 20},
    {name: 'Kalisz  Mikstat', range: 100},
    {name: 'Słupsk  ul. Banacha', range: 20},
    {name: 'Kalisz  Chełmce', range: 20},
    {name: 'Przemyśl  Tatarska Góra', range: 20},
    {name: 'Żagań  Wichów', range: 50},
    {name: 'Przedbórz', range: 20},
    {name: 'Opole  Korfantego', range: 20},
    {name: 'Dobromierz', range: 20},
    {name: 'Leżajsk  Giedlarowa', range: 20},
    {name: 'Czyże', range: 20},
    {name: 'Lublin  Boży Dar', range: 20},
    {name: 'Różan', range: 20},
    {name: 'Poznań  Piątkowo', range: 20},
    {name: 'Tomaszów Mazowiecki  ul. Mościckiego', range: 20},
    {name: 'Nowa Ruda  g. Krępiec', range: 20},
    {name: 'Koszalin  Góra Chełmska', range: 20},
    {name: 'Dobiegniew', range: 20},
    {name: 'Bardo Śląskie  wzg. Różańcowe', range: 20},
    {name: 'Chodzież  ul. Strzelecka 17', range: 20},
    {name: 'Wałbrzych  Chełmiec', range: 20},
    {name: 'Katowice  Rybnik', range: 20},
    {name: 'Tarnów  g. Św. Marcina', range: 20},
    {name: 'Płock  Rachocin', range: 20},
    {name: 'Katowice  Kosztowy', range: 20},
    {name: 'Zgorzelec  ul. Górna', range: 20},
    {name: 'Kowary  g. Rudnik', range: 20},
    {name: 'Stronie Śląskie  os. Morawka', range: 20},
    {name: 'Płock  Rachocin', range: 100},
    {name: 'Gorzów Wielkopolski  ul. Podmiejska Boczna 21', range: 20},
    {name: 'Siedlce  Łosice', range: 100},
    {name: 'Choczewo  Żelazno', range: 20},
    {name: 'Szymbark', range: 20},
    {name: 'Istebna  g. Złoty Groń', range: 20},
    {name: 'Komańcza  g. Krymieniec', range: 20},
    {name: 'Krzemienna  g. Mały Dział', range: 20},
    {name: 'Solina  g. Jawor', range: 20},
    {name: 'Rzeszów  Sucha Góra', range: 100},
    {name: 'Łapsze Wyżne  g. Grandeus', range: 20},
    {name: 'Ochotnica Dolna  g. Koci Zamek', range: 100},
    {name: 'Rabka  Luboń Wielki', range: 20},
    {name: 'Szczawnica  g. Jarmuta', range: 20},
    {name: 'Szczawnica  g. Przehyba', range: 22},
    {name: 'Tylmanowa  g. Matuszek', range: 20},
    {name: 'Winiary', range: 20},
    {name: 'Brodnica  Kruszynki', range: 20},
    {name: 'Gryfice  ul. Trzygłowska', range: 20},
    {name: 'Sandomierz  ul. Mokoszyńska', range: 20},
    {name: 'Ciechanów  ul. Monte Cassino', range: 20},
    {name: 'Łódź  EC', range: 170},
    {name: 'Łomża  Szosa Zambrowska', range: 20},
    {name: 'Leżajsk  Giedlarowa', range: 70},
    {name: 'Mieroszów  wzg. Cmentarne', range: 20},
    {name: 'Szczytna  Szklana Góra', range: 20},
    {name: 'Bogatynia  g. Wysoka', range: 20},
    {name: 'Leśna  wzg. Baworowo', range: 20},
    {name: 'Kłodzko  Czarna Góra', range: 50},
    {name: 'Bolewice', range: 20},
    {name: 'Lubań  Nowa Karczma', range: 20},
    {name: 'Włocławek Zazamcze  ul. Szpitalna 30', range: 20},
    {name: 'Wrocław  Żórawina', range: 20},
    {name: 'Gorlice  Maślana Góra', range: 20},
    {name: 'Mielnik  ul. Popław', range: 20},
    {name: 'Szczecin  Kołowo', range: 100},
    {name: 'Częstochowa  Błeszno', range: 20},
    {name: 'Głogów  Jakubów', range: 20},
    {name: 'Legnica  ul. Piastowska', range: 20},
    {name: 'Brenna  Wzgórze Janty', range: 20},
    {name: 'Dynów  g. Winnica', range: 20},
    {name: 'Majdan', range: 20},
    {name: 'Polana  g. Szeroka Łąka', range: 20},
    {name: 'Racibórz  ul. Cmentarna', range: 20},
    {name: 'Rymanów Zdrój  wzn. Zamczyska', range: 20},
    {name: 'Solina  g. Plasza', range: 20},
    {name: 'Stuposiany  g. Czereszna', range: 20},
    {name: 'Szczyrk  ul. Salmopolska', range: 20},
    {name: 'Ustroń  g. Czantoria', range: 20},
    {name: 'Męcina  g. Wysokie', range: 20},
    {name: 'Wierzbica Górna', range: 20},
    {name: 'Tymbark  g. Podłopień', range: 20},
    {name: 'Zawoja  g. Miśkowcowa', range: 20},
    {name: 'Trzebiatów  ul. Wodna', range: 20},
    {name: 'Nowe Miasto Lubawskie  Kurzętnik', range: 20},
    {name: 'Człuchów  ul. Szkolna', range: 20},
    {name: 'Toruń  Grębocin Cergia', range: 20},
    {name: 'Białystok  Krynice', range: 100},
    {name: 'Chruszczewka', range: 20},
    {name: 'Kazimierz Dolny  Góry I', range: 20},
    {name: 'Lublin  Piaski', range: 100},
    {name: 'Suwałki  Krzemianucha', range: 20},
    {name: 'Kłodzko  Czarna Góra', range: 20},
    {name: 'Lębork  Skórowo Nowe', range: 20},
    {name: 'Rzeszów  Sucha Góra', range: 20},
    {name: 'Rabka  Luboń Wielki', range: 20},
    {name: 'Piła  Rusinowo', range: 40},
    {name: 'Łomża  Szosa Zambrowska', range: 20},
    {name: 'Siedlce  ul. Błonie', range: 20},
    {name: 'Olsztyn  Pieczewo', range: 60},
    {name: 'Bydgoszcz  Trzeciewiec', range: 20},
    {name: 'Wrocław  Ślęża', range: 20},
    {name: 'Warszawa  PKiN', range: 20},
    {name: 'Wałbrzych  Chełmiec', range: 20},
    {name: 'Łódź  Zygry', range: 25},
    {name: 'Poznań  Śrem', range: 22},
    {name: 'Elbląg  Jagodnik', range: 20},
    {name: 'Leżajsk  Giedlarowa', range: 20},
    {name: 'Szczawnica  g. Przehyba', range: 20},
    {name: 'Gorlice  Maślana Góra', range: 20},
    {name: 'Zakopane  Gubałówka', range: 20},
    {name: 'Kamieńsk  Zwałowisko', range: 20},
    {name: 'Grzywacz', range: 20},
    {name: 'Jelenia Góra  Śnieżne Kotły', range: 20},
    {name: 'Gdańsk  Chwaszczyno', range: 20},
    {name: 'Żagań  Wichów', range: 20},
    {name: 'Szymbark', range: 20},
    {name: 'Toruń  Grębocin Cergia', range: 20},
    {name: 'Kielce  Święty Krzyż', range: 20},
    {name: 'Bogatynia  g. Wysoka', range: 20},
    {name: 'Koszalin  Gołogóra', range: 20},
    {name: 'Konin  Żółwieniec', range: 20},
    {name: 'Włocławek  pl. Wolności', range: 20},
    {name: 'Gniezno  Chojna', range: 40},
    {name: 'Chruszczewka', range: 20},
    {name: 'Strzeżów  Miechów', range: 20},
    {name: 'Białogard  Sławoborze', range: 25},
    {name: 'Skierniewice  Bartniki', range: 20},
    {name: 'Czyże', range: 20},
    {name: 'Kraków  Chorągwica', range: 20},
    {name: 'Lublin  Piaski', range: 35},
    {name: 'Zamość  Tarnawatka', range: 40},
    {name: 'Ciechanów  ul. Monte Cassino', range: 20},
    {name: 'Giżycko  Miłki', range: 20},
    {name: 'Solina  g. Jawor', range: 20},
    {name: 'Tarnów  g. Św. Marcina', range: 20},
    {name: 'Zielona Góra  Jemiołów', range: 22},
    {name: 'Płock  Radziwie', range: 25},
    {name: 'Wisła  Skrzyczne', range: 20},
    {name: 'Trzebnica  g. Farna', range: 20},
    {name: 'Iława  Kisielice', range: 20},
    {name: 'Szczecin  Kołowo', range: 20},
    {name: 'Lublin  Boży Dar', range: 30},
    {name: 'Siedlce  Łosice', range: 20},
    {name: 'Przemyśl  Tatarska Góra', range: 20},
    {name: 'Poznań  AE', range: 20},
    {name: 'Wysoka  g. Św. Anny', range: 20},
    {name: 'Człuchów  ul. Szkolna', range: 20},
    {name: 'Częstochowa  Wręczyca Wielka', range: 20},
    {name: 'Głobikowa', range: 20},
    {name: 'Radom  ul. Przytycka', range: 20},
    {name: 'Płock  Rachocin', range: 20},
    {name: 'Głogów  Jakubów', range: 20},
    {name: 'Białystok  Krynice', range: 24},
    {name: 'Zduny', range: 20},
    {name: 'Oborniki  Uścikowo', range: 20},
    {name: 'Katowice  Kosztowy', range: 20},
    {name: 'Kalisz  Chełmce', range: 20},
    {name: 'Suwałki  Krzemianucha', range: 20},
    {name: 'Ostrołęka  ul. Kopernika', range: 20},
    {name: 'Dobromierz', range: 20},
    {name: 'Przysucha  Kozłowiec', range: 20},
    {name: 'Kobyla Góra  Parzynów', range: 20},
    {name: 'Dęblin  Ryki', range: 20},
    {name: 'Cieszyn  ul. Mickiewicza', range: 20},
    {name: 'Tarnobrzeg  Machów', range: 20},
    {name: 'Gdańsk  Chwaszczyno', range: 20},
    {name: 'Warszawa  PKiN', range: 20},
    {name: 'Piła  Rusinowo', range: 50},
    {name: 'Leżajsk  Giedlarowa', range: 20},
    {name: 'Konin  Żółwieniec', range: 50},
    {name: 'Kielce  Święty Krzyż', range: 25},
    {name: 'Łódź  EC', range: 40},
    {name: 'Lublin  Piaski', range: 50},
    {name: 'Bydgoszcz  Trzeciewiec', range: 50},
    {name: 'Wrocław  Ślęża', range: 100},
    {name: 'Gdańsk  Chwaszczyno', range: 100},
    {name: 'Białystok  Krynice', range: 20},
    {name: 'Tarnów  g. Św. Marcina', range: 20},
    {name: 'Żagań  Wichów', range: 25},
    {name: 'Grzywacz', range: 20},
    {name: 'Gorlice  Maślana Góra', range: 20},
    {name: 'Jelenia Góra  Śnieżne Kotły', range: 100},
    {name: 'Katowice  Kosztowy', range: 30},
    {name: 'Poznań  Śrem', range: 40},
    {name: 'Solina  g. Jawor', range: 20},
    {name: 'Lębork  Skórowo Nowe', range: 20},
    {name: 'Szczawnica  g. Przehyba', range: 20},
    {name: 'Wysoka  g. Św. Anny', range: 20},
    {name: 'Elbląg  Jagodnik', range: 20},
    {name: 'Siedlce  Łosice', range: 40},
    {name: 'Przysucha  Kozłowiec', range: 20},
    {name: 'Zielona Góra  Jemiołów', range: 30},
    {name: 'Warszawa  Raszyn', range: 50},
    {name: 'Wisła  Skrzyczne', range: 20},
    {name: 'Kalisz  Mikstat', range: 20},
    {name: 'Częstochowa  Wręczyca Wielka', range: 25},
    {name: 'Rzeszów  Sucha Góra', range: 40},
    {name: 'Suwałki  Krzemianucha', range: 20},
    {name: 'Iława  Kisielice', range: 20},
    {name: 'Ostrołęka  Ławy', range: 50},
    {name: 'Ciechanów  ul. Monte Cassino', range: 20},
    {name: 'Płock  Rachocin', range: 50},
    {name: 'Rabka  Luboń Wielki', range: 20},
    {name: 'Giżycko  Miłki', range: 25},
    {name: 'Koszalin  Gołogóra', range: 50},
    {name: 'Zakopane  Gubałówka', range: 20},
    {name: 'Zamość  Tarnawatka', range: 50},
    {name: 'Dęblin  Ryki', range: 20},
    {name: 'Przemyśl  Tatarska Góra', range: 20},
    {name: 'Białogard  Sławoborze', range: 20},
    {name: 'Szczecin  Kołowo', range: 100},
    {name: 'Kłodzko  Czarna Góra', range: 20},
    {name: 'Gniezno  Chojna', range: 20},
    {name: 'Olsztyn  Pieczewo', range: 50},
    {name: 'Kraków  Chorągwica', range: 80},
];

import fetch from "node-fetch";
import fs from 'fs';
import { resourceLimits } from "worker_threads";

const textToId = async text => {
    const formattedText = encodeURIComponent(text);
    const request =
`https://maps.googleapis.com/maps/api/place/findplacefromtext/json?key=${API_KEY}&inputtype=textquery&input=${formattedText}`;
    const result = await fetch(request);
    if(!result.ok) return;
    return ((await result.json()).candidates[0].place_id);
};

const idToLocation = async id => {
    const request = `https://maps.googleapis.com/maps/api/place/details/json?place_id=${id}&key=${API_KEY}`;
    const result = await fetch(request);
    if(!result.ok) return;
    return ((await result.json()).result.geometry.location);
};

const nameToLocation = async name => {
    try{
        return await idToLocation(await textToId(name));
    } catch(e) {
        //console.log(e);
        return {lat: 0, lng: 0};
    }
};

const saveFile = async (filename, data) => {
    fs.writeFile(filename, JSON.stringify(data), function(err) {
        if(err) {
            return console.log(err);
        }
        //console.log("The file was saved!");
    });
}

(async () => {
    let result = [];
    
    for(let i = 0; i < data.length; i++) {
        const position = await nameToLocation(data[i].name);
        console.log(position);
        result.push({name: data[i].name, position, range: data[i].range});
    }

    saveFile(`StationaryStations.json`, result);
})();

