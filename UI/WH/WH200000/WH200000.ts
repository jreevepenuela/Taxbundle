import { Messages as SysMessages } from "client-controls/services/messages";
import {
  createCollection,
  createSingle,
  PXScreen,
  graphInfo,
  PXActionState,
  viewInfo,
  handleEvent,
  CustomEventType,
  actionConfig,
  RowSelectedHandlerArgs,
  PXViewCollection,
  PXPageLoadBehavior,
  ControlParameter,
  PXView,
  PXFieldState,
  gridConfig,
  treeConfig,
  fieldConfig,
  controlConfig,
  headerDescription,
  ICurrencyInfo,
  disabled,
  PXFieldOptions,
  linkCommand,
  columnConfig,
  GridColumnShowHideMode,
  GridColumnType,
  TextAlign,
  GridPreset,
  GridFilterBarVisibility,
  GridFastFilterVisibility,
  ISelectorControlConfig,
} from "client-controls";

@graphInfo({ graphType: "TaxBundle.WHTaxMaint", primaryView: "Tax" })
export class WH200000 extends PXScreen {
  Tax = createCollection(WHTax);
}
// Views

@gridConfig({
  autoAdjustColumns: true,
  showFastFilter: GridFastFilterVisibility.False,
  mergeToolbarWith: "ScreenToolbar",
  preset: GridPreset.Primary,
})
export class WHTax extends PXView {
  @columnConfig({
    width: 280,
  })
  WithholdingCD: PXFieldState;
  @columnConfig({
    width: 280,
  })
  Type: PXFieldState;
  @columnConfig({
    width: 100,
  })
  TaxRate: PXFieldState;
  @columnConfig({
    width: 280,
  })
  Description: PXFieldState;
  @columnConfig({
    width: 280,
  })
  Birform: PXFieldState;
}
