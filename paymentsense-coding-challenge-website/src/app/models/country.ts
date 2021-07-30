import { Currency } from "./currency";
import { Language } from "./language";

export class Country {
  name: string;
  flag: string;
  population: number;
  timezones: string[];
  currencies: Currency[];
  languages: Language[];
  capital: string;
}
