# Game of life
Questa è la implementazione per il "**Game of life di Conway**" ([http://en.wikipedia.org/wiki/Conway%27s_Game_of_Life](http://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)).

Le regole sono:
1. Qualsiasi cella viva con meno di due celle vive adiacenti muore, come per effetto d'isolamento;
2. Qualsiasi cella viva con più di tre celle vive adiacenti muore, come per effetto di sovrappopolazione;
3. Qualsiasi cella viva con due o tre celle vive adiacenti sopravvive alla generazione successiva;
4. Qualsiasi cella morta con esattamente tre celle vive adiacenti diventa una cella viva, come per effetto di riproduzione.

Ho implementato sia una rapresentazione console più basica, sia una rappresentazione web con blazor con un minimo di stile grafico e qualche configurazione aggiuntiva.

Non conoscevo questo gioco, è interessante che si sono trovate configurazioni per la crescita illimitata come il "Cannone di alianti di Gosper". 
Ho aggiunto anche qualche test per verificare le configurazioni stabili come il "Blinker" e il blocco 2x2.

Spero che la mia soluzione vi piaccia! 😄

