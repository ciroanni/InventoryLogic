# InventoryLogic

L'obiettivo principale di questo lavoro è implementare la logica di un sistema dinventario in modo chiaro

Nota importante:
- A livello scena è stato usato il playground dello [Starter Assets Third Person](https://assetstoreunitycom/packages/essentials/starter-assets-thirdperson-urp-196526) di Unity
- Mi sono concentrato soprattutto sulla logica inventario, non su una UI production-ready per tutti i dispositivi
- Non sono espertissimo di UI, quindi il comportamento su risoluzioni/aspect ratio molto diversi potrebbe non essere bellissimo

## Come funziona il sistema

### 1) Inventario

Il centro di tutto è `InventorySystem`:
- Numero slot configurabile (`slotNumbers`, attualmente 18)
- Aggiunta item con stacking su slot già compatibili
- Riempimento automatico degli slot vuoti
- Rimozione item con decremento quantità
- Swap tra slot

### 2) Dati item

`ItemData` è uno ScriptableObject con:
- Nome e descrizione
- Icona
- `MaxStack`
- Lista di effetti (`List<ItemEffect>`)

Ogni item può avere più effetti contemporaneamente

### 3) Effetti item

Gli effetti derivano da `ItemEffect` e implementano `Apply(ItemEffectContext)`

Esempi presenti nel progetto:
- `TimedPlayerStatEffect`: applica moltiplicatori temporanei alle stat del player

`ItemEffectContext` passa agli effetti le info utili (utente, transform, inventario, item, slot)

Per gli effetti temporanei viene usato `EffectRuntimeController`, aggiunto runtime al player se necessario

### 4) UI inventario

`InventoryUI`:
- Crea dinamicamente gli slot UI in base al numero slot logici
- Si aggiorna quando cambia l'inventario
- Gestisce apertura/chiusura inventario
- Quando è aperto: blocca i controlli del player e abilita il cursore
- Quando è chiuso: riabilita i controlli e rilocka il cursore

`InventorySlotUI`:
- Visualizza icona e quantità
- Drag & drop tra slot per lo swap
- Click destro per usare l'item

## Controlli principali

- `Tab`: apre/chiude inventario
- Drag & drop con mouse: scambio tra slot
- Click destro su slot: usa item

## Asset usati

- La scena è il Playground dello starter asset 
- Gli oggetti in scena al momento sono primitive geometriche
- Le icone UI sono prese da [qui](https://assetstoreunitycom/packages/2d/gui/icons/gui-parts-159068)

## Limiti e miglioramenti

Limiti attuali:
- UI pensata principalmente per il task logico, non ancora ottimizzata per ogni device
- Pochi item

Miglioramenti futuri:
- Rifinitura UI responsive (scaling, anchor, leggibilità multi-risoluzione)
- Tooltips item (nome, descrizione, effetti)


## Stato del progetto

Il sistema è già utilizzabile per testare:
- raccolta/aggiunta item
- stacking
- uso item con effetti
- interazioni base UI (apertura, drag & drop, uso)

Il focus principale resta la logica inventario, come richiesto dal task
