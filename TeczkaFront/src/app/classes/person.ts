export class Person {
    id: number | undefined;
    last?: string;
    first?: string;
    userId? : number;
    classId?: number;
}

export class PersonUpdate {
    last?: string;
    first?: string;
    userId?: number;
    classId?: number;
}
