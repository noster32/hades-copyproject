package com.example.backend.model;

import lombok.Data;
import lombok.ToString;

@ToString
@Data
public class UserModel {

    private String userId;
    private String userPw;
    private String userNm;
    private String userMail;

    private String inputDtm;
    private String inputNm;

    private String modDtm;
    private String modNm;
}
