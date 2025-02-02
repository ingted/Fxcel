﻿namespace Fxcel

open Midoliy.Office.Interop
open System.Drawing

module CellOp =
  type PasteMode = { Paste: PasteType; Op: PasteOperation; SkipBlanks: bool; Transpose: bool; }
  type InsertMode = { Shift: InsertShiftDirection; Origin: InsertFormatOrigin; }
  type DeleteMode = { Shift: DeleteShiftDirection; }

  type CellOpBuilder () =
    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    [<CustomOperation("copy")>]
    member __.Copy(_: unit, target: IExcelRange) = target.Copy() |> ignore
    [<CustomOperation("paste")>]
    member __.Paste(_: unit, target: IExcelRange, pasteMode: PasteMode) = target.Paste(pasteMode.Paste, pasteMode.Op, pasteMode.SkipBlanks, pasteMode.Transpose) |> ignore
    [<CustomOperation("insert")>]
    member __.Insert(_: unit, target: IExcelRange, insertMode: InsertMode) = target.Insert(insertMode.Shift, insertMode.Origin) |> ignore
    [<CustomOperation("delete")>]
    member __.Delete(_: unit, target: IExcelRange, deleteMode: DeleteMode) = target.Delete(deleteMode.Shift) |> ignore
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, value: IExcelRange) = target.Value <- value.Value
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, value: obj) = target.Value <- value
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, color: Color) = target.Interior.Color <- color
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, theme: ThemeColor) = target.Interior.ThemeColor <- theme
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, pattern: Pattern) = target.Interior.Pattern <- pattern
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, halign: HorizontalAlignment) = target.HorizontalAlignment <- halign
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: IExcelRange, valign: VerticalAlignment) = target.VerticalAlignment <- valign
    [<CustomOperation("fx")>]
    member __.Fx(_: unit, target: IExcelRange, value: IExcelRange) = target.Formula <- value.Formula
    [<CustomOperation("fx")>]
    member __.Fx(_: unit, target: IExcelRange, value: string) = target.Formula <- if (string value).StartsWith("=") then value else $"={value}"
    [<CustomOperation("width")>]
    member __.SetWidth(_: unit, target: IExcelRange, length: int) = target.ColumnWidth <- length
    [<CustomOperation("height")>]
    member __.SetHeight(_: unit, target: IExcelRange, length: int) = target.RowHeight <- length
    [<CustomOperation("fit'width")>]
    member __.FitWidth(_: unit, target: IExcelRange) = target.EntireColumn.AutoFit()
    [<CustomOperation("fit'height")>]
    member __.FitHeight(_: unit, target: IExcelRange) = target.EntireRow.AutoFit()
    [<CustomOperation("merge")>]
    member __.Merge(_: unit, target: IExcelRange, across: bool) = target.Merge(across)
    [<CustomOperation("unmerge")>]
    member __.UnMerge(_: unit, target: IExcelRange) = target.UnMerge()
    [<CustomOperation("wrap")>]
    member __.WrapText(_: unit, target: IExcelRange) = target.WrapText <- true
    [<CustomOperation("unwrap")>]
    member __.UnWrapText(_: unit, target: IExcelRange) = target.WrapText <- false
    [<CustomOperation("shrink")>]
    member __.ShrinkToFit(_: unit, target: IExcelRange) = target.ShrinkToFit <- true
    [<CustomOperation("unshrink")>]
    member __.UnShrinkToFit(_: unit, target: IExcelRange) = target.ShrinkToFit <- false
    [<CustomOperation("orientation")>]
    member __.Orientation(_: unit, target: IExcelRange, angle: int) = target.Orientation <- angle
    [<CustomOperation("format")>]
    member __.Format(_: unit, target: IExcelRange, format: string) = target.Format <- format
    [<CustomOperation("select")>]
    member __.Select(_: unit, target: IExcelRange) = target.Select()

  type CellOpBuilder2 (sheet: IWorksheet) =
    member __.Yield (_: unit) = ()
    member __.Zero() = ()
    [<CustomOperation("copy")>]
    member __.Copy(_: unit, target: string) = sheet.[target].Copy() |> ignore
    [<CustomOperation("paste")>]
    member __.Paste(_: unit, target: string, pasteMode: PasteMode) = sheet.[target].Paste(pasteMode.Paste, pasteMode.Op, pasteMode.SkipBlanks, pasteMode.Transpose) |> ignore
    [<CustomOperation("insert")>]
    member __.Insert(_: unit, target: string, insertMode: InsertMode) = sheet.[target].Insert(insertMode.Shift, insertMode.Origin) |> ignore
    [<CustomOperation("delete")>]
    member __.Delete(_: unit, target: string, deleteMode: DeleteMode) = sheet.[target].Delete(deleteMode.Shift) |> ignore
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, value: IExcelRange) = sheet.[target].Value <- value.Value
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, value: obj) = sheet.[target].Value <- value
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, color: Color) = sheet.[target].Interior.Color <- color
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, theme: ThemeColor) = sheet.[target].Interior.ThemeColor <- theme
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, pattern: Pattern) = sheet.[target].Interior.Pattern <- pattern
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, halign: HorizontalAlignment) = sheet.[target].HorizontalAlignment <- halign
    [<CustomOperation("set")>]
    member __.Set(_: unit, target: string, valign: VerticalAlignment) = sheet.[target].VerticalAlignment <- valign
    [<CustomOperation("fx")>]
    member __.Fx(_: unit, target: string, value: IExcelRange) = sheet.[target].Formula <- value.Formula
    [<CustomOperation("fx")>]
    member __.Fx(_: unit, target: string, value: string) = sheet.[target].Formula <- if (string value).StartsWith("=") then value else $"={value}"
    [<CustomOperation("width")>]
    member __.SetWidth(_: unit, target: string, length: int) = sheet.[target].ColumnWidth <- length
    [<CustomOperation("height")>]
    member __.SetHeight(_: unit, target: string, length: int) = sheet.[target].RowHeight <- length
    [<CustomOperation("fit'width")>]
    member __.FitWidth(_: unit, target: string) = sheet.[target].EntireColumn.AutoFit()
    [<CustomOperation("fit'height")>]
    member __.FitHeight(_: unit, target: string) = sheet.[target].EntireRow.AutoFit()
    [<CustomOperation("merge")>]
    member __.Merge(_: unit, target: string, across: bool) = sheet.[target].Merge(across)
    [<CustomOperation("unmerge")>]
    member __.UnMerge(_: unit, target: string) = sheet.[target].UnMerge()
    [<CustomOperation("wrap")>]
    member __.WrapText(_: unit, target: string) = sheet.[target].WrapText <- true
    [<CustomOperation("unwrap")>]
    member __.UnWrapText(_: unit, target: string) = sheet.[target].WrapText <- false
    [<CustomOperation("shrink")>]
    member __.ShrinkToFit(_: unit, target: string) = sheet.[target].ShrinkToFit <- true
    [<CustomOperation("unshrink")>]
    member __.UnShrinkToFit(_: unit, target: string) = sheet.[target].ShrinkToFit <- false
    [<CustomOperation("orientation")>]
    member __.Orientation(_: unit, target: string, angle: int) = sheet.[target].Orientation <- angle
    [<CustomOperation("format")>]
    member __.Format(_: unit, target: string, format: string) = sheet.[target].Format <- format
    [<CustomOperation("select")>]
    member __.Select(_: unit, target: string) = sheet.[target].Select()

  [<System.Obsolete("You should to change from cell'op to use sheet'op.")>]
  let cell'op = CellOpBuilder ()
  let cell'op2 sheet = CellOpBuilder2 sheet
  let paste'mode = { Paste= PasteType.All; Op= PasteOperation.None; SkipBlanks= false; Transpose= false; }
  let insert'mode = { Shift= InsertShiftDirection.Down; Origin= InsertFormatOrigin.FromRightOrBelow; }
  let delete'mode = { Shift= DeleteShiftDirection.Left; }

