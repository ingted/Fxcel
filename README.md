# Fxcel - Excel operations library  

![Fxcel](https://raw.githubusercontent.com/tatsuya-midorikawa/Fxcel/main/assets/fxcel.png)  


## 🔷 What's this?  

- Fxcel は F# で簡単に Excel の COM 操作をするためのライブラリです。  
  - C# 向けの Excel COM 操作ライブラリである ***[Midoliy.Office.Interop.Excel](https://github.com/Midoliy/Midoliy.Office.Interop.Excel)*** のラッパーライブラリとなります。
- .NET 5.0 以上の環境をサポートしています。  
- 主に F# Script や F# Interactive での利用を想定して設計をしていますが、Console アプリや Desktop アプリでも問題なく利用可能です。  
- COM を利用するため Excel のインストールが必要です。  

---

## 🔷 Get started  

### ◼◻ F# Interactiveで利用する

#### 1. FSIを起動する  

```powershell
dotnet fsi
```

#### 2. Fxcel を読み込む

Fxcel を nuget から読み込みます。

```fsharp
#r "nuget: Fxcel, 0.0.21";;
open Fxcel;;
```  

### ◼◻ F# Scriptで利用する

#### 1. **.fsx** ファイルを作成する  

まずはコーディングを始めるために **main.fsx** を作成して、VSCode で開きましょう。  

```powershell
mkdir D:/work
cd D:/work
new-item main.fsx
code D:/work
```

#### 2. Fxcel を読み込む

**main.fsx** に Fxcel を利用するためのコードを追加します。

```fsharp
#r "nuget: Fxcel"
open Fxcel
```  

### ◼◻ F# プロジェクトで利用する

#### 1. 新規プロジェクトを作成する  

```powershell
mkdir D:/work
cd D:/work
dotnet new console -lang=F# -o=FxcelSample
``` 
#### 2. Fxcel を読み込む 

```powershell
cd D:/work/FxcelSample
dotnet add package Fxcel
``` 

---

## 🔷 Reference for F# Interactive

### ◼◻ 起動中のExcelプロセス一覧をターミナルに表示しつつ取得する<br>`show ()`

```fsharp
let processList = show ();;
```

### ◼◻ 起動中のExcelプロセス一覧を取得する<br>`enumerate ()`

```fsharp
let processList = enumerate ();;
```

### ◼◻ 起動中のExcelプロセスにアタッチする<br>`attach (excel: Handle)`

```fsharp
let processList = enumerate ();;
let excel = processList.[0] |> attach;;
```

### ◼◻ アタッチ済みのExcelプロセスをデタッチする<br>`detach (excel: IExcelApplication)`

```fsharp
let processList = enumerate ();;
let excel = processList.[0] |> attach;;

// do somethings

excel |> detach;;
```

---

## 🔷 Reference  

### ◼◻ Workbookを新規作成する<br>`create (): IExcelApplication`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = create ()
```

### ◼◻ 既存Workbookをテンプレートとして新規Workbookを作成する<br>`create (template: string): IExcelApplication`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = create "C:/work/sample.xlsx"
```

### ◼◻ 既存Workbookを開く<br>`open' (filepath: string): IExcelApplication`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
```

### ◼◻ Workbookを名前を付けて保存する<br>`saveAs (filepath: string) (excelObject: ^ExcelObject): unit`

```fsharp
[<EntryPoint>]
let main argv =
  // Workbookを新規作成し, ExcelApplicationを取得.
  use excel = create()

  // (1) Workbookを利用して, 名前を付けて保存.
  let book = excel |> workbook(1)
  // do somethings
  book |> saveAs "C:/work/sample.xlsx"

  // (2) Worksheetを利用して, 名前を付けて保存.
  let sheet = excel |> workbook(1) |> worksheet(1)
  // do somethings
  sheet |> saveAs "C:/work/sample.xlsx"
```

### ◼◻ Workbookを上書き保存する<br>`save (excelObject: ^ExcelObject): unit`

```fsharp
[<EntryPoint>]
let main argv =
  // 既存のExcelワークブックを開く.
  use excel = open' "C:/work/sample.xlsx"

  // (1) Workbookを利用して, 上書き保存.
  let book = excel |> workbook(1)
  // do somethings
  book |> save

  // (2) Worksheetを利用して, 上書き保存.
  let sheet = excel |> workbook(1) |> worksheet(1)
  // do somethings
  sheet |> save
```

### ◼◻ Workbookを取得する<br>`workbook (index: int) (excel: IExcelApplication): IWorkbook`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"

  // indexを指定してWorkbookを取得.
  //   -> index は 1 始まりであることに注意する.
  let book = excel |> workbook(1)
```

### ◼◻ Worksheetを取得する<br>`worksheet (index: int | string) (workbook: IWrokbook): IWorksheet`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"

  // (1) indexを指定してWorksheetを取得.
  //   -> index は 1 始まりであることに注意する.
  let sheet = excel |> workbook(1) |> worksheet(1)

  // (2) sheet nameを指定して取得.
  let sheet = excel |> workbook(1) |> worksheet("Sheet1")
```

### ◼◻ Worksheetを新規追加する<br>`newsheet (book: IWorkbook): IWorksheet`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> newsheet
```

### ◼◻ IExcelRangeオブジェクトを取得する<br>`sheet.[address]: IExcelRange`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // (1) アドレス形式で取得.
  let cell = sheet.["A1"]
  // (2) R1C1形式で取得.
  let cell = sheet.[1, 1]
  // (3) 範囲をアドレス形式で取得.
  let range = sheet.["A1:B3"]
  // (4) 範囲を始点セルアドレスと終点セルアドレスを指定して取得.
  let range = sheet.["A1", "B3"]
```

### ◼◻ IWorksheetから行を取得する<br>`get'row (index: int) (sheet: IWorksheet): IExcelRow`<br>`get'rows (begin': int, end': int) (sheet: IWorksheet): IExcelRows`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 1行取得.
  let r = sheet |> get'row(1)       // $1:$1
  // 複数行取得.
  let r = sheet |> get'rows(1, 3)   // $1:$3
```

### ◼◻ IWorksheetから列を取得する<br>`get'column (index: int) (sheet: IWorksheet): IExcelRow`<br>`get'columns (begin': int, end': int) (sheet: IWorksheet): IExcelRows`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 1列取得.
  let c = sheet |> get'column(1)       // $A:$A
  // 複数列取得.
  let c = sheet |> get'columns(1, 3)   // $A:$C
```

### ◼◻ IExcelRangeの行全体を取得する<br>`current'rows (range: IExcelRange): IExcelRows`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 行全体を取得.
  let r = sheet.["A1"] |> current'rows      // $1:$1
  let r = sheet.["A1:B3"] |> current'rows   // $1:$3
```

### ◼◻ IExcelRangeの列全体を取得する<br>`current'columns (range: IExcelRange): IExcelColumns`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 列全体を取得.
  let r = sheet.["A1"] |> current'columns      // $A:$A
  let r = sheet.["A1:B3"] |> current'columns   // $A:$B
```

### ◼◻ 範囲データを行ごとに列挙する<br>`rows (range: IExcelRange): seq<IExcelRow>`<br>`rowsi (range: IExcelRange): seq<int * IExcelRow>`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // rows関数を利用して, 1行ずつ取得.
  for row in sheet.["A1:B3"] |> rows do
    // 各Cell毎に何か処理.
    for cell in row do
      // do somethings


  // rowsi関数を利用して, インデックス付きで1行ずつ取得.
  //   -> index は 1 始まりであることに注意.
  for (index, row) in sheet.["A1:B3"] |> rowsi do
    // 各Cell毎に何か処理.
    for cell in row do
      // do somethings
```

### ◼◻ 範囲データを列ごとに列挙する<br>`columns (range: IExcelRange): seq<IExcelColumn>`<br>`columnsi (range: IExcelRange): seq<int * IExcelColumn>`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // columns関数を利用して, 1行ずつ取得.
  for column in sheet.["A1:B3"] |> columns do
    // 各Cell毎に何か処理.
    for cell in column do
      // do somethings


  // columnsi関数を利用して, インデックス付きで1行ずつ取得.
  //   -> index は 1 始まりであることに注意.
  for (index, column) in sheet.["A1:B3"] |> columnsi do
    // 各Cell毎に何か処理.
    for cell in column do
      // do somethings
```

### ◼◻ 値を取得する<br>`get (cell: IExcelRange): obj`<br>`get<'T> (cell: IExcelRange): 'T`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 値を取得.
  let v: obj = sheet.["A1"] |> get

  // 値を型付きで取得.
  //   -> 指定した型と互換性がない場合, System.InvalidCastException.
  let v: int = sheet.["A1"] |> get<int>

  // 複数要素がある場合は先頭要素の値のみ取得.
  //   -> 以下の場合 sheet.["A1"] の値が得られる.
  let v: obj = sheet.["A1:B3"] |> get

  // 複数要素がある場合は先頭要素の型付きの値のみ取得.
  //   -> 以下の場合 sheet.["A1"] の値が得られる.
  //   -> 指定した型と互換性がない場合, System.InvalidCastException.
  let v: int = sheet.["A1:B3"] |> get<int>
```

### ◼◻ 値を配列データで取得する<br>`gets (range: IExcelRange): obj [,]`<br>`gets<'T> (range: IExcelRange): 'T [,]`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 値を配列データとして取得.
  let vs: obj [,]  = sheet.["A1:A3"] |> gets

  // 値を型付きの配列データとして取得.
  //   -> 指定した型と互換性がない場合, System.InvalidCastException.
  let vs: int [,]  = sheet.["A1:A3"] |> gets<int>

  // 取得した配列データから先頭要素の値を取得.
  //   -> 以下の場合 sheet.["A1"] の値が得られる.
  let v: obj = sheet.["A1:B3"] |> gets |> head

  // 取得した型付きの配列データから先頭要素の値を取得.
  //   -> 以下の場合 sheet.["A1"] の値が得られる.
  //   -> 指定した型と互換性がない場合, System.InvalidCastException.
  let v: int = sheet.["A1:B3"] |> gets<int> |> head

  // 取得した配列データから末尾要素の値を取得.
  //   -> 以下の場合 sheet.["B3"] の値が得られる.
  let v: obj = sheet.["A1:B3"] |> gets |> last

  // 取得した型付きの配列データから末尾要素の値を取得.
  //   -> 以下の場合 sheet.["B3"] の値が得られる.
  //   -> 指定した型と互換性がない場合, System.InvalidCastException.
  let v: int = sheet.["A1:B3"] |> gets<int> |> last
```

### ◼◻ 関数を取得する<br>`getfx (cell: IExcelRange): string`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 関数を取得.
  let fn: string = sheet.["A1"] |> getfx
```

### ◼◻ 関数を配列データで取得する<br>`getsfx (range: IExcelRange): string [,]`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 関数を配列データで取得.
  let fns: string [,] = sheet.["A1:A3"] |> getsfx
```

### ◼◻ 値を設定する<br>`set (value: obj) (target: IExcelRange): unit`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 値を設定.
  sheet.["A1"] |> set 100
  sheet.["A1:B3"] |> set 100
```

### ◼◻ 関数を設定する<br>`fx (func: string) (target: IExcelRange): unit`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 関数を設定.
  sheet.["A1"] |> fx "SUM(A2:A5)"
  sheet.["A1:B3"] |> fx "COUNT(A1:B3)"
```

### ◼◻ 背景色を設定する<br>`bgcolor (color: Color) (target: IExcelRange): unit`

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 背景色を設定.
  sheet.["A1"] |> bgcolor Color.Red
  sheet.["B1:B3"] |> bgcolor Color.Blue
  sheet.["C1"] |> bgcolor (rgb(0, 128, 255))
```

### ◼◻ 背景パターンを設定する<br>`bgpattern (pattern: Pattern) (target: IExcelRange): unit`

| arg name | values |
| --- | --- |
| pattern | `pattern'none`<br>`pattern'auto`<br>`pattern'up`<br>`pattern'down`<br>`pattern'vertical`<br>`pattern'horizontal`<br>`pattern'lightUp`<br>`pattern'lightDown`<br>`pattern'lightVertical`<br>`pattern'lightHorizontal`<br>`pattern'gray8`<br>`pattern'gray16`<br>`pattern'gray25`<br>`pattern'gray50`<br>`pattern'gray75`<br>`pattern'semigray75`<br>`pattern'solid`<br>`pattern'checker`<br>`pattern'grid`<br>`pattern'crisscross`<br>`pattern'linearGradient`<br>`pattern'rectangularGradient` |

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 背景パターンを設定.
  sheet.["A1"] |> bgpattern pattern'checker
  sheet.["B1:B3"] |> bgpattern pattern'crisscross
```

### ◼◻ 罫線を設定する<br>`ruledline (target: IExcelRange): IBorders` コンピュテーション式

#### 📑 `ruledline` で利用できるカスタムオペレーション

| operation name | description |
| --- | --- |
| `top (border)` | 最上部の横罫線. |
| `bottom (border)` | 最下部の横罫線. |
| `left (border)` | 最左部の縦罫線. |
| `right (border)` | 最右部の縦罫線. |
| `horizontal (border)` | 中間部の横罫線. |
| `vertical (border)` | 中間部の縦罫線. |
| `growing (border)` | 左下から右上に向けての罫線. 色や太さの設定は `falling` と共有. |
| `falling (border)` | 左上から右下に向けての罫線. 色や太さの設定は `growing` と共有. |

#### 📑 `Border` に設定できる値

| operation name | description | values |
| --- | --- | --- |
| `LineStyle` | 罫線のスタイル.<br>**default: `linestyle'continuous`** | `linestyle'none`<br>`linestyle'dot`<br>`linestyle'double`<br>`linestyle'dash`<br>`linestyle'continuous`<br>`linestyle'dashdot`<br>`linestyle'dashdotdot`<br>`linestyle'slant`|
| `Weight` | 罫線の太さ.<br>**default: `weight'medium`** | `weight'medium`<br>`weight'hairline`<br>`weight'thin`<br>`weight'thick` |
| `Color` | 罫線の色.<br>**default: `Color.Black`** | `Color.Red`<br>`Color.Orange`<br>`Color.Blue`<br>`rgb(r, g, b)`<br>and more... |

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // 罫線を設定.
  ruledline sheet.["B2:C5"] {
    // 各 Border の値は with を利用して指定.
    top { border with Color= Color.Red }
    left { border with Color= Color.Orange; Weight= weight'thick }
    right { border with LineStyle= linestyle'dashdot }
    bottom { border with Weight= weight'medium }
    horizontal { border with Color= Color.Blue; Weight= weight'medium }
    vertical { border with Color= rgb (0, 128, 255); Weight= weight'thin }

    // growing と falling は値がExcel内部で共有されているため、設定値は後勝ちする.
    growing { border with Weight= weight'hairline }
    falling { border with Weight= weight'thick }
  }
  |> ignore
```

### ◼◻ フォントを設定する / `font (target: IExcelRange): IRangeFont` コンピュテーション式

#### 📑 `font` で利用できるカスタムオペレーション

| operation name | description | values |
| --- | --- | --- |
| `set (fontName: string)`<br>`name (fontName: string)` | フォント名. | `游ゴシック`<br>`メイリオ`<br>`consolas`<br>and more... |
| `set (size: float)`<br>`size (size: float)` | フォントサイズ. | `8.0`<br>`10.5`<br>`24.0`<br>and more... |
| `set (style: FontStyle)` | フォントスタイル. `Flags` なので複数まとめて指定可能. | `style'normal`<br>`style'bold`<br>`style'italic'`<br>`style'shadow`<br>`style'strikethrough`<br>`style'subscript`<br>`style'superscript`<br>`style'singleUnderline`<br>`style'doubleUnderline` |
| `set (value: Color)` | フォント色. | `Color.Red`<br>`Color.Orange`<br>`Color.Blue`<br>and more... |
| `set (value: RGB)` | フォント色. | `rgb(r: int, g: int, b: int)`<br>`{ r= 0; g= 128; b= 255; }` |
| `set (style: Underline)` | 下線. | `underline'none`<br>`underline'double`<br>`underline'doubleAccounting`<br>`underline'single`<br>`underline'singleAccounting` |
| `bold (on: bool)` | 太字. | `true` or `false` |
| `italic (on: bool)` | イタリック体. | `true` or `false` |
| `shadow (on: bool)` | フォント影. | `true` or `false` |
| `outline (on: bool)` | アウトラインフォント. | `true` or `false` |
| `strikethrough (on: bool)` | 打ち消し線. | `true` or `false` |
| `subscript (on: bool)` | 下付き文字にする. | `true` or `false` |
| `superscript (on: bool)` | 上付き文字にする. | `true` or `false` |

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)

  // フォントを設定.
  font sheet.["A1:A3"] {
    // フォントの指定.
    set "メイリオ"  // or
    name "メイリオ"
    // フォントサイズの設定.
    set 16.0  // or
    size 16.0
    // 下線の設定.
    set underline'double

    // フォント色の設定.
    set Color.Orange          // or
    set ( rgb(0, 128, 255) )  // or
    set { r= 0; g= 128; b= 255; }

    // フォントスタイルの設定.
    set style'normal
    // スタイルを複数選択する場合は以下のように指定.
    set (style'normal ||| style'strikethrough ||| style'shadow)
    // style を直接指定しなくとも各種スタイルをひとつずつ ON/OFF 可能.
    bold true
    italic true
    shadow true
    outline true
    strikethrough true
    subscript true
    superscript true
  }
  |> ignore
```

### ◼◻ IExcelApplicationオブジェクトを操作する<br>`excel'op (excel: IExcelApplication)` コンピュテーション式

#### 📑 `excel'op` で利用できるカスタムオペレーション

| operation name | description | values |
| --- | --- | --- |
| `set (mode: Calculation)` | Excelの再計算制御を設定する.<br>**default: `calc'manual`** | `calc'auto`<br>`calc'manual`<br>`calc'semiauto` |
| `set (visibility: AppVisibility)` | Excelの表示状態を設定する.<br>**default: `visibility'hidden`** | `visibility'visible`<br>`visibility'hidden` |

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  
  excel'op excel {
    set calc'semiauto
    set visibility'visible
  }
```

### ◼◻ IExcelRangeオブジェクトを操作する（コピー・ペースト・挿入・削除など）<br>`sheet'op (sheet): IWorksheet)` コンピュテーション式

#### 📑 `sheet'op` で利用できるカスタムオペレーション

| operation name | description | values |
| --- | --- | --- |
| `copy (target: string)` | 対象をクリップボードにコピーする. | - |
| `paste (target: string, pasteMode: PasteMode)` | 対象にクリップボードの値を貼り付ける. | - |
| `insert (target: string, insertMode: InsertMode)` | 対象にクリップボードの値を挿入する. | - |
| `delete (target: string, deleteMode: DeleteMode)` | 対象を削除する. | - |
| `set (target: string, value: obj)` | 対象に値を設定する. | - |
| `set (target: string, color: Color)` | 対象の背景色を設定する. | `Color.Red`<br>`Color.Orange`<br>`Color.Blue`<br>and more... |
| `set (target: string, theme: ThemeColor)` | 対象の背景色をテーマカラーで設定する. | `theme'background1`<br>`theme'background2`<br>`theme'foreground1`<br>`theme'foreground2`<br>`theme'accent1`<br>`theme'accent2`<br>`theme'accent3`<br>`theme'accent4`<br>`theme'accent5`<br>`theme'accent6`<br> |
| `set (target: string, pattern: Pattern)` | 対象の背景パターンを設定する. | `pattern'none`<br>`pattern'auto`<br>`pattern'up`<br>`pattern'down`<br>`pattern'vertical`<br>`pattern'horizontal`<br>`pattern'lightUp`<br>`pattern'lightDown`<br>`pattern'lightVertical`<br>`pattern'lightHorizontal`<br>`pattern'gray8`<br>`pattern'gray16`<br>`pattern'gray25`<br>`pattern'gray50`<br>`pattern'gray75`<br>`pattern'semigray75`<br>`pattern'solid`<br>`pattern'checker`<br>`pattern'grid`<br>`pattern'crisscross`<br>`pattern'linearGradient`<br>`pattern'rectangularGradient` |
| `set (target: string, halign: HorizontalAlignment)` |文字の水平位置を設定する. | `h'right`<br>`h'left`<br>`h'center`<br>`h'justify`<br>`h'distributed`<br>`h'general`<br>`h'fill`<br>`h'centerAcrossSelection` |
| `set (target: string, valign: VerticalAlignment)` |文字の垂直位置を設定する. | `v'top`<br>`v'bottom`<br>`v'center `<br>`v'justify`<br>`v'distributed` |
| `fx (target: string, formula: string)` | 対象に関数を設定する. | - |
| `width (target: string, length: int)` | 対象の列幅をpt単位で設定する. | - |
| `height (target: string, length: int)` | 対象の行高をpt単位で設定する. | - |
| `fit'width (target: string)` | 対象の列幅を自動調整する. | - |
| `fit'height (target: string)` | 対象の行高を自動調整する. | - |
| `merge (target: string, across: bool)` | セルを結合する. | `true`: 範囲内のセルを行ごとに結合.<br>`false`: 範囲内すべてのセルを1つに結合. |
| `unmerge (target: string)` | セルの結合を解除する. | - |
| `wrap (target: string)` | 折り返して全体を表示する. | - |
| `unwrap (target: string)` | 折り返して全体を表示を解除する. | - |
| `shrink (target: string)` | 縮小して全体を表示する. | - |
| `unshrink (target: string)` | 縮小して全体を表示を解除する. | - |
| `orientation (target: string, angle: int)` | 文字の方向を設定する. | -90° ~ 90° |
| `format (target: string, format: string)` | セルの値の表示形式を設定する. | - |

#### 📑 `PasteMode` の要素

| name | description | values |
| --- | --- | --- |
| `Paste` | 貼り付け方式.<br>**default: `paste'all`** | `paste'values`<br>`paste'comments`<br>`paste'formulas`<br>`paste'formats`<br>`paste'all`<br>`paste'validation`<br>`paste'exceptBorders`<br>`paste'colmnWidths`<br>`paste'formulasAndNumberFormats`<br> `paste'valuesAndNumberFormats`<br>`paste'allUsingSourceTheme`<br>`paste'allMergingConditionalFormats` |
| `Op` | 貼り付け時の演算方法.<br>**default: `op'none`** | `op'none`<br>`op'add`<br>`op'sub`<br>`op'mul`<br>`op'div` |
| `SkipBlanks` | 空白セルを無視するか.<br>**default: `false`** | `true` or `false` |
| `Transpose` | 行列を入れ替えるか.<br>**default: `false`** | `true` or `false` |

#### 📑 `InsertMode` の要素

| name | description | values |
| --- | --- | --- |
| `Shift` | 挿入後に他のセルをどうシフト移動するか.<br>**default: `shift'down`** | `shift'right`<br>`shift'down` |
| `Origin` | 書式をコピーしてくる方向.<br>**default: `origin'right`, `origin'below`** | `origin'left`<br>`origin'above`<br>`origin'right`<br>`origin'below` |

#### 📑 `DeleteMode` の要素

| name | description | values |
| --- | --- | --- |
| `Shift` | 削除後に他のセルをどうシフト移動するか. | `shift'left`<br>`shift'up` |


```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)
  
  sheet'op sheet {
    // A1 をクリップボードにコピー.
    copy "A1"
    // 範囲コピーも可能.
    copy "A1:A3"

    // クリップボードのデータを B1 に貼り付け.
    paste "B1" paste'mode
    paste "B1" { paste'mode with Paste= paste'values }
    paste "B1" { paste'mode with Op= op'add }
    paste "B1" { paste'mode with SkipBlanks= true }
    paste "B1" { paste'mode with Transpose= true }
    paste "B1" { paste'mode with Paste= paste'values; SkipBlanks= true }
    paste "B1" { paste'mode with Paste= paste'values; Transpose= true }
    // 範囲貼り付けも可能.
    paste "B1:B3" paste'mode

    // クリップボードのデータを C1 に挿入.
    insert "C1" insert'mode
    insert "C1" { insert'mode with Shift= shift'down }
    insert "C1" { insert'mode with Origin= origin'below }
    insert "C1" { insert'mode with Shift= shift'right; Origin= origin'below }
    // 範囲挿入も可能.
    insert "C1:C3" insert'mode

    // A1 のデータを削除する.
    delete "A1" delete'mode
    delete "A1" { delete'mode with Shift= shift'up }
    // 範囲削除も可能.
    delete "A1:A3" delete'mode

    // A1 に値を設定.
    set "A1" 100
    set "A1" sheet.["B1"]

    // A1 に関数を設定.
    fx "A1" "SUM(A1:B3)"
    fx "A1" sheet.["B1"]
    
    // 列幅を 100 に設定.
    width "A1" 100
    width "A1:B3" 100
    // 行高を 100 に設定.
    height "A1" 100
    height "A1:B3" 100

    // 列幅を自動調整.
    fit'width "A1"
    fit'width "A1:B3"
    // 行高を自動調整.
    fit'height "A1"
    fit'height "A1:B3"

    // 背景色を設定.
    set "A1" Color.Blue
    // 背景色をテーマカラーで設定.
    set "A1" theme'accent1
    // 背景パターンを設定.
    set "A1" pattern'horizontal

    // セルを結合.
    merge "C1:D3" true
    merge "E1:F3" false
    // セルの結合を解除.
    unmerge "C1:D3"
    unmerge "E1:F3"

    // 折り返して全体を表示.
    wrap "A1"
    // 折り返して全体を表示を解除.
    unwrap "A1"

    // 縮小して全体を表示.
    shrink "A1"
    // 縮小して全体を表示を解除.
    unshrink "A1"

    // 文字の方向を設定.
    orientation "A1" -90
    orientation "A1" 0
    orientation "A1" 90

    // 表示形式を設定.
    format "A1" "(日付)yyyy-MM-dd"
  }
```

### ◼◻ チャートやグラフを挿入する<br>`chart'op (sheet: IWorksheet)` コンピュテーション式

#### 📑 `chart'op` で利用できるカスタムオペレーション

| operation name | description | values |
| --- | --- | --- |
| `select (target: string)` | チャートで利用するデータの範囲を選択する. | - |
| `size (size: (int<cols> * int<rows>))` | チャートのサイズをセル数で指定する. (幅 * 高さ). | - |
| `position (position: string)` | チャートを挿入する位置を指定する. | - |
| `add (recipe: ChartRecipe, ?newLayout: bool)` | チャートを追加する. | `newLayout`: 動的書式設定規則を使用する (default: true). |

#### 📑 `ChartRecipe` の要素

| name | description |
| --- | --- |
| `columnClustered` | 集合縦棒 |
| `columnStacked` | 積み上げ縦棒 |
| `columnStacked100` | 100% 積み上げ縦棒 |
| `barStacked` | 積み上げ横棒 |
| `barStacked100` | 100% 積み上げ横棒 |
| `column3d` | 3-D 縦棒 |
| `columnClustered3d` | 3-D 集合縦棒 |
| `coneCol` | 3-D 円錐型縦棒 |
| `coneColClustered` | 集合円錐型 縦棒 |
| `cylinderCol` | 3-D 円柱型 縦棒 |
| `cylinderColClustered` | 集合円錐型 縦棒 |
| `pyramidCol` | 3-D ピラミッド型縦棒 |
| `pyramidColClustered` | 集合ピラミッド型 縦棒 |
| `columnStacked3d` | 3-D 積み上げ縦棒 |
| `columnStacked3d100` | 3-D 100% 積み上げ縦棒 |
| `cylinderColStacked` | 積み上げ円錐型 縦棒 |
| `cylinderColStacked100` | 100% 積み上げ円柱型 縦棒 |
| `cylinderBarStacked` | 積み上げ円柱型 横棒 |
| `cylinderBarStacked100` | 100% 積み上げ円柱型 横棒 |
| `coneColStacked` | 積み上げ円錐型 縦棒 |
| `coneColStacked100` | 100% 積み上げ円錐型 縦棒 |
| `coneBarStacked` | 積み上げ円錐型 横棒 |
| `coneBarStacked100` | 100% 積み上げ円錐型 横棒 |
| `pyramidColStacked` | 積み上げピラミッド型 縦棒 |
| `pyramidColStacked100` | 100% 積み上げピラミッド型 縦棒 |
| `pyramidBarStacked` | 積み上げピラミッド型 横棒 |
| `pyramidBarStacked100` | 100% 積み上げピラミッド型 横棒 |
| `barStacked3d` | 3-D 積み上げ横棒 |
| `barStacked3d100` | 3-D 100% 積み上げ横棒 |
| `barClustered` | 集合横棒 |
| `barClustered3d` | 3-D 集合横棒 |
| `cylinderBarClustered` | 集合円柱型 横棒 |
| `coneBarClustered` | 集合円錐型 横棒 |
| `pyramidBarClustered` | 集合ピラミッド型 横棒 |
| `lineStacked` | 積み上げ折れ線 |
| `lineStacked100` | 100% 積み上げ折れ線 |
| `lineMarkersStacked100` | マーカー付き 100% 積み上げ折れ線 |
| `line` | 折れ線 |
| `lineMarkers` | マーカー付き折れ線 |
| `lineMarkersStacked` | マーカー付き積み上げ折れ線 |
| `pieOfPie` | 補助円グラフ付き円 |
| `barOfPie` | 補助縦棒グラフ付き円 |
| `doughnut` | ドーナツ |
| `doughnutExploded` | 分割ドーナツ |
| `pie` | 円 |
| `pieExploded` | 分割円 |
| `pie3d` | 3-D 円 |
| `pieExploded3d` | 分割 3-D 円 |
| `xyScatter` | 散布図 |
| `xyScatterSmooth` | 平滑線付き散布図 |
| `xyScatterSmoothNoMarkers` | 平滑線付き散布図（データ マーカーなし） |
| `xyScatterLines` | 折れ線付き散布図 |
| `xyScatterLinesNoMarkers` | 折れ線付き散布図（データ マーカーなし） |
| `area` | 面 |
| `areaStacked` | 積み上げ面 |
| `areaStacked100` | 100% 積み上げ面 |
| `area3d` | 3-D 面 |
| `areaStacked3d` | 3-D 積み上げ面 |
| `areaStacked1003d` | 3-D 100% 積み上げ面 |
| `radar` | レーダー |
| `radarMarkers` | データ マーカー付きレーダー |
| `radarFilled` | 塗りつぶしレーダー |
| `surface` | 3-D 表面 |
| `surfaceWireframe` | 3-D 表面（ワイヤーフレーム） |
| `surfaceTopView` | 表面（トップビュー） |
| `surfaceTopViewWireframe` | 表面（トップビュー・ワイヤーフレーム） |
| `line3d` | 3-D 折れ線 |
| `bubble` | バブル |
| `bubble3dEffect` | 3-D 効果付きバブル |
| `stockHLC` | 高値-安値-終値 |
| `stockOHLC` | 始値-高値-安値-終値 |
| `stockVHLC` | 出来高-高値-安値-終値 |
| `stockVOHLC` | 出来高-始値-高値-安値-終値 |
| `sunburst` | サンバースト |


```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)
  
  sheet.["A1:F1"] |> set [| 100; 120; 110; 100; 200; 180; |]

  chart'op sheet {
    select "A1:F1"
    position "A2"
    size (6<cols>, 10<rows>)
    add ChartRecipe.line
  } |> ignore

```

### ◼◻ IExcelRangeを削除する<br>`delete (direction: DeleteShiftDirection) (target: IExcelRange): unit`

#### 📑 `DeleteShiftDirection`

| value | description |
| --- | --- |
| `shift'left` | 削除後, 左方向へシフト. |
| `shift'up` | 削除後, 上方向へシフト. |

```fsharp
[<EntryPoint>]
let main argv =
  use excel = open' "C:/work/sample.xlsx"
  let sheet = excel |> workbook(1) |> worksheet(1)
  
  // 対象を削除.
  sheet.["A1"] |> delete shift'up
  sheet.["A1:A3"] |> delete shift'left
```

---

## 🔷 Utility  

### ◼◻ 数値をカラム名に変換する<br>`column'name (index: int): string`

```fsharp
let name = 1 |> column'name     // A
let name = 10 |> column'name    // J
let name = 128 |> column'name   // DX
```

### ◼◻ カラム名をインデックスに変換する<br>`column'number (column: string): int`

```fsharp
let number = "A" |> column'number     // 1
let number = "J" |> column'number     // 10
let number = "DX" |> column'number    // 128
```

### ◼◻ IExcelRangeからアドレスを取得する<br>`address (target: IExcelRange): string`

```fsharp
let adds = sheet.["A1"] |> address      // $A$1
let adds = sheet.["A1:B3"] |> address   // $A$1:$B$3
```

### ◼◻ ExcelObjectを選択する<br>`activate (target: ^T): unit`

```fsharp
// Workbookを選択状態にする.
excel |> workbook(1) |> activate

// Worksheetを選択状態にする.
excel |> workbook(1) |> worksheet(1) |> activate

// Cellを選択状態にする.
sheet.["B1"] |> activate
sheet.["A1:B3"] |> activate
```

### ◼◻ ExcelObjectを選択する<br>`select (target: ^T): unit`

```fsharp
// Worksheet(1)を選択状態にする.
excel |> workbook(1) |> worksheet(1) |> select

// Cellを選択状態にする.
sheet.["B1"] |> select
sheet.["D1:E3"] |> select
```

---

## 🔷 TIPS  

### ◼◻ `try-with` の利用  

例外処理を施していない場合 Excel COM オブジェクトが適切に解放されず, プロセス上に残ってしまう恐れがあります.  
`try-with` (または `try-with-finally`) と `use` を併用することで Excel COM オブジェクトの解放漏れを防げます.  

```fsharp
try
  // use を利用する.
  use excel = create ()

  // do somethings

with
  _ -> ()
```  

また, F# Interactive で利用する場合, `attach` したあとは必ず `detach` する必要があります.  

```powershell
let ps = enumerate ();;
let excel = ps.[0] |> attach;;

# do somethings

excel |> detach;;
```

### ◼◻ `LangVersion` の指定  

**.NET 5** を利用している場合, うまくコンピュテーション式が動作しない可能性があります.  
その場合は `LangVersion` に **preview** を指定するようにしてください.

```xml
<!-- .fsproj を書き換える場合 -->
<PropertyGroup>
  <LangVersion>preview</LangVersion>
</PropertyGroup>
```

```powershell
# dotnet fsi コマンドにオプションを指定する場合
dotnet fsi --langversion:preview
```
