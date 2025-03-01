package com.example.backend.model;

import lombok.Data;
import lombok.ToString;

@Data
@ToString
public class AchivementModel {
    private int achiveId;
    private String achiveNm;
    private String achiveDesc;
    private char useYn;
}
