/* eslint-disable @stylistic/indent */
import {
  PXFieldState,
  PXFieldOptions,
  PXView,
  updateDecorator,
  removeDecorator,
  featureInstalled,
  FeaturesSet,
  placeBeforeView,
  placeAfterView,
  placeBeforeProperty,
  placeAfterProperty,
  graphInfo,
  viewInfo,
  gridConfig,
  columnConfig,
  treeConfig,
  controlConfig,
  linkCommand,
  fieldOptions,
  fieldConfig,
  fieldInfo,
  headerDescription,
  autoRefresh,
  mappedToViewField,
  suppressLabel,
  readOnly,
  disabled,
  unbound,
  text,
  multiLine,
  primaryKey,
  type,
  ControlParameter,
  NetType,
  MenuItemRenderType,
  ScrollMode,
  TextAlign,
  GridPreset,
  GridColumnType,
  GridColumnShowHideMode,
  GridColumnDisplayMode,
  GridAutoGrowMode,
  GridPagerMode,
  GridFastFilterVisibility,
  GridNoteFilesShowMode,
  HeaderDescription,
  GridColumnGeneration,
  GridFilterBarVisibility,
  NodeSelectMode,
  PXSelectorMode,
  ApplySuggestionMode,
  ITextEditorControlConfig,
  ITimeSpanConfig,
  ISelectorControlConfig,
  ISegmentedSelectorControlConfig,
  IEditorControlConfig,
  INumberEditorControlConfig,
  IMaskEditorControlConfig,
  IMailEditorControlConfig,
  ILinkEditorControlConfig,
  IDropDownConfig,
  IDatetimeEditControlConfig,
  IColorPickerControlConfig,
  ICheckBoxControlConfig,
  ICalendarControlConfig,
  IBranchSelectorConfig,
  IBarcodeInputControlConfig,
  ITreeSelectorConfig,
  IRichTextEditorConfig,
  IFormulaEditorConfig,
  ICurrencyControlConfig,
  FieldGenerationMode,
} from "client-controls";
import { AR301000, ARTran } from "src/screens/AR/AR301000/AR301000";

export interface AR301000_TaxBundle extends AR301000 {}
export class AR301000_TaxBundle {}

export interface ARTran_TaxBundle extends ARTran {}
export class ARTran_TaxBundle {
  UsrTaxAmount: PXFieldState;

  UsrTaxDescription: PXFieldState;

  UsrTaxRate: PXFieldState;

  UsrWithholdingCD: PXFieldState;
}
