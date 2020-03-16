export class CreateJobRequest {
    referenceLabourInMinutes: number;
    referencePrice: number;

    items: IJobItem[] = [];
}

export enum WheelPosition {
    NearsideFront,
    NearsideRear,
    OffsideFront,
    OffsideRear
}

export interface IJobItem {
    $type: string;
}

export class TyreReplacement implements IJobItem {
    $type: string = "TyreReplacement";
    position: WheelPosition;
    size: number;
}

export class BrakeDiscReplacement implements IJobItem {
    $type: string;
    position: WheelPosition;
}

export class BrakePadReplacement implements IJobItem {
    $type: string;
    position: WheelPosition;
}

export class ExhaustReplacement implements IJobItem {
    $type: string;
}

export class OilChange implements IJobItem {
    $type: string;
}