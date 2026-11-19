import React from "react";
import {
  MenuItem,
  TextField,
} from "@material-ui/core";
import useAvaluoComercialStyles from "../styles/avaluoComercial.styles";
import {
  FormValues,
  SelectOption,
} from "../types/avaluoComercial.types";

type Props = {
  name: keyof FormValues;
  label: string;
  value: string;
  error?: string;
  type?: string;
  select?: boolean;
  options?: SelectOption[];
  readOnly?: boolean;
  required?: boolean;
  onChange: (
    field: keyof FormValues,
    value: string
  ) => void;
};

function CampoAvaluo({
  name,
  label,
  value,
  error = "",
  type = "text",
  select = false,
  options = [],
  readOnly = false,
  required = true,
  onChange,
}: Props) {
  const classes =
    useAvaluoComercialStyles();

  const handleChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    if (readOnly) {
      return;
    }

    onChange(
      name,
      event.target.value
    );
  };

  return (
    <TextField
      className={classes.field}
      fullWidth
      required={required}
      label={label}
      value={value || ""}
      type={type}
      select={select}
      error={Boolean(error)}
      helperText={error || ""}
      onChange={handleChange}
      InputProps={{
        readOnly,
      }}
      InputLabelProps={
        type === "date"
          ? {
              shrink: true,
            }
          : undefined
      }
    >
      {select &&
        options.map((option) => (
          <MenuItem
            key={option.value}
            value={option.value}
          >
            {option.label}
          </MenuItem>
        ))}
    </TextField>
  );
}

export default CampoAvaluo;