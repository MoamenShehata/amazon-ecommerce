export interface CountryLookup {
  id: number;
  name: string;
  cities: CityLookup[];
}

export interface CityLookup {
  id: number;
  name: string;
}
