export interface PersonDto {
    personId: number;
    name: string;
    currentRank: string;
    currentDutyTitle: string;
    careerStartDate: Date | undefined;
    careerEndDate: Date | undefined;
  }